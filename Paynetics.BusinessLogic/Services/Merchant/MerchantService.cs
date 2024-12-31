using Microsoft.EntityFrameworkCore;
using Paynetics.BusinessLogic.Models;
using Paynetics.Data;
using Paynetics.Data.Entities;
using System.Text;

namespace Paynetics.BusinessLogic.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly PayneticsDbContext _dbContext;

        public MerchantService(PayneticsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedResponse<Data.Entities.Merchant>> GetMerchantsAsync(MerchantFilter filter)
        {
            IQueryable<Merchant> query = _dbContext.Merchants.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(m => m.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.StartBoardingDate.HasValue)
            {
                query = query.Where(m => m.BoardingDate >= filter.StartBoardingDate.Value);
            }

            if (filter.EndBoardingDate.HasValue)
            {
                query = query.Where(m => m.BoardingDate <= filter.EndBoardingDate.Value);
            }

            if (!string.IsNullOrEmpty(filter.Url))
            {
                query = query.Where(m => m.Url.Contains(filter.Url, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(filter.Country))
            {
                query = query.Where(m => m.Country.Contains(filter.Country, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(filter.Address1))
            {
                query = query.Where(m => m.Address1.Contains(filter.Address1, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(filter.Address2))
            {
                query = query.Where(m => m.Address2.Contains(filter.Address2, StringComparison.OrdinalIgnoreCase));
            }

            int totalCount = await query.CountAsync();

            var merchants = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            int totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

            return new PaginatedResponse<Merchant>
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = filter.PageNumber,
                Data = merchants
            };
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
                var batch = await _dbContext.Merchants
                                            .Skip(skip)
                                            .Take(batchSize)
                                            .ToListAsync();
                if (batch.Count < batchSize)
                {
                    hasMoreData = false;
                }

                foreach (var entity in batch)
                {
                    var line = $"{entity.Id},{entity.Name},{entity.Country},{entity.Url},{entity.Address1},{entity.Address2},{entity.BoardingDate}";
                    await writer.WriteLineAsync(line);
                }

                skip += batchSize;
            }

            await writer.FlushAsync();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
    }
}
