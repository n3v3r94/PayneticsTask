using Paynetics.BusinessLogic.Models;

namespace Paynetics.BusinessLogic.Services
{
    public interface IMerchantService
    {
        Task<PaginatedResponse<Data.Entities.Merchant>> GetMerchantsAsync(MerchantFilter filter);
        Task<MemoryStream> ExportToCsvAsync();
    }
}
