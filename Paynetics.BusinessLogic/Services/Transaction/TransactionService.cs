using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Paynetics.BusinessLogic.Models;
using Paynetics.BusinessLogic.Models.Transaction;
using Paynetics.Data;
using Paynetics.Data.Entities;
using Paynetics.Data.Enum;
using System.Text;

namespace Paynetics.BusinessLogic.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly PayneticsDbContext _dbContext;

        public TransactionService(PayneticsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ProceedBatchAsync(TransactionBatch batch)
        {

            var partners = await _dbContext.Partners
                .Include(x => x.Merchants)
                .Where(x => batch.Debts.Contains(x.Name))
                .ToDictionaryAsync(x => x.Name);

            var newPartners = new List<Partner>();
            var newMerchants = new List<Merchant>();
            var transactions = new List<Transaction>();

            foreach (var item in batch.Transactions)
            {
                var transaction = new Transaction
                {
                    Amount = item.Amount.Value,
                    Currency = item.Amount.Currency,
                    Direction = item.Amount.Direction,
                    BeneficiaryIban = item.Beneficiary.IBAN,
                    CreateDate = item.CreateDate,
                    DebtorIban = item.Debtor.IBAN,
                    ExternalId = item.ExternalId,
                    Status = (TransactionStatus)item.Status,
                };

                if (!partners.ContainsKey(item.Debtor?.BankName))
                {
                    var merchant = new Merchant
                    {
                        Name = item.Beneficiary.BankName,
                        BoardingDate = DateTimeOffset.Now
                    };

                    var partner = new Partner
                    {
                        Name = item.Debtor.BankName,
                        Merchants = new HashSet<Merchant> { merchant }
                    };

                    merchant.Partner = partner;
                    transaction.Merchant = merchant;

                    newPartners.Add(partner);
                    partners[item.Debtor?.BankName] = partner;
                }
                else
                {
                    var partner = partners[item.Debtor?.BankName];
                    var merchant = partner.Merchants.FirstOrDefault(x => x.Name == item.Beneficiary.BankName);

                    if (merchant == null)
                    {
                       
                        merchant = new Merchant
                        {
                            Name = item.Beneficiary.BankName,
                            BoardingDate = DateTimeOffset.Now,
                            Partner = partner
                        };

                        partner.Merchants.Add(merchant);
                        newMerchants.Add(merchant);
                    }

                    transaction.Merchant = merchant;
                }

                transactions.Add(transaction);
            }

           
            if (newPartners.Any())
            {
                await _dbContext.Partners.AddRangeAsync(newPartners);
                await _dbContext.SaveChangesAsync(); 
            }

            if (newMerchants.Any())
            {
                await _dbContext.Merchants.AddRangeAsync(newMerchants);
                await _dbContext.SaveChangesAsync(); 
            }

            if (transactions.Any())
            {
                await _dbContext.BulkInsertOrUpdateAsync(transactions, new BulkConfig { IncludeGraph = true });
            }
        }

        public async Task<MemoryStream> ExportToCsvAsync()
        {
            const int batchSize = 10000;
            int skip = 0;
            bool hasMoreData = true;

            var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true);

            while (hasMoreData)
            {
                var batch = await _dbContext.Transactions
                                            .Skip(skip)
                                            .Take(batchSize)
                                            .ToListAsync();

                if (batch.Count < batchSize)
                {
                    hasMoreData = false;
                }

                foreach (var entity in batch)
                {
                    var line = $"{entity.Id},{entity.CreateDate:yyyy-MM-dd HH:mm:ss},{entity.Amount},{entity.Currency},{entity.Direction}," +
                        $"{entity.BeneficiaryIban},{entity.DebtorIban},{entity.ExternalId},{entity.Status}";
                    await writer.WriteLineAsync(line);
                }

                skip += batchSize;
            }

            await writer.FlushAsync();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public async Task<PaginatedResponse<Transaction>> GetTransactionsAsync(TransactionFilter filter)
        {
            IQueryable<Transaction> query = _dbContext.Transactions.AsQueryable();

            if (filter.StartDate.HasValue)
            {
                query = query.Where(t => t.CreateDate >= filter.StartDate.Value);
            }
            if (filter.EndDate.HasValue)
            {
                query = query.Where(t => t.CreateDate <= filter.EndDate.Value);
            }
            if (filter.MinAmount.HasValue)
            {
                query = query.Where(t => t.Amount >= filter.MinAmount.Value);
            }
            if (filter.MaxAmount.HasValue)
            {
                query = query.Where(t => t.Amount <= filter.MaxAmount.Value);
            }
            if (!string.IsNullOrEmpty(filter.Direction))
            {
                query = query.Where(t => t.Direction.Equals(filter.Direction, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(filter.Currency))
            {
                query = query.Where(t => t.Currency.Equals(filter.Currency, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(filter.DebtorIban))
            {
                query = query.Where(t => t.DebtorIban.Equals(filter.DebtorIban, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(filter.BeneficiaryIban))
            {
                query = query.Where(t => t.BeneficiaryIban.Equals(filter.BeneficiaryIban, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.Status.HasValue)
            {
                query = query.Where(t => t.Status == filter.Status.Value);
            }

            int totalCount = await query.CountAsync();

            var transactions = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            int totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

            return new PaginatedResponse<Transaction>
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = filter.PageNumber,
                Data = transactions
            };
        }
    }
}
