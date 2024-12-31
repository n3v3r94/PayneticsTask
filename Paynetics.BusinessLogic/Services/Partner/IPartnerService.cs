using Paynetics.BusinessLogic.Models;
using Paynetics.Data.Entities;

namespace Paynetics.BusinessLogic.Services
{
    public interface IPartnerService
    {
        Task<PaginatedResponse<Partner>> GetPartnersAsync(PartnerFilter filter);
        Task<MemoryStream> ExportToCsvAsync();
    }
}
