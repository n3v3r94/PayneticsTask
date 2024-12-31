using Microsoft.EntityFrameworkCore;
using Paynetics.BusinessLogic.Models;
using Paynetics.Data;
using Paynetics.Data.Entities;
using System.Text;

namespace Paynetics.BusinessLogic.Services
{
    public class PartnerService : IPartnerService
    {
        private readonly PayneticsDbContext _dbContext;

        public PartnerService(PayneticsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedResponse<Partner>> GetPartnersAsync(PartnerFilter filter)
        {
            IQueryable<Partner> query = _dbContext.Partners.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(p => p.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
            }

            int totalCount = await query.CountAsync();

            var partners = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            int totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

            return new PaginatedResponse<Partner>
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = filter.PageNumber,
                Data = partners
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
                var batch = await _dbContext.Partners
                                            .Skip(skip)
                                            .Take(batchSize)
                                            .ToListAsync();

                if (batch.Count < batchSize)
                {
                    hasMoreData = false;
                }

                foreach (var entity in batch)
                {
                    var line = $"{entity.Id},{entity.Name}";
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
