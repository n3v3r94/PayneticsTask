using Microsoft.AspNetCore.Mvc;
using Paynetics.BusinessLogic.Models;
using Paynetics.BusinessLogic.Services;

namespace Paynetics.API.Controllers
{
    public class PartnerContoller : APIControllerBase
    {
        private readonly IPartnerService partnerService;
        public PartnerContoller(IPartnerService partnerService)
        {
            this.partnerService = partnerService;
        }

        [HttpGet("filter-partners")]
        public async Task<IActionResult> FilterTransaction([FromQuery] PartnerFilter filter)
        {
            try
            {
                var data = await partnerService.GetPartnersAsync(filter);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error get partners: {ex.Message}");
            }
        }

        [HttpGet("export-csv")]
        public async Task<IActionResult> ExportToCsv()
        {
            try
            {
                var csvStream = await partnerService.ExportToCsvAsync();
                return File(csvStream, "text/csv", $"partners-{DateTimeOffset.UtcNow.Ticks}.csv");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error export  csv file: {ex.Message}");
            }
        }

    }
}
