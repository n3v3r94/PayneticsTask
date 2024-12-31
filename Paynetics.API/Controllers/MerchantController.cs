using Microsoft.AspNetCore.Mvc;
using Paynetics.BusinessLogic.Models;
using Paynetics.BusinessLogic.Services;

namespace Paynetics.API.Controllers
{
    public class MerchantController : APIControllerBase
    {
        private readonly IMerchantService merchantService;

        public MerchantController(IMerchantService merchantService)
        {
            this.merchantService = merchantService;
        }

        [HttpGet("filter-merchants")]
        public async Task<IActionResult> FilterTransaction([FromQuery] MerchantFilter filter)
        {
            try
            {
                var data = await merchantService.GetMerchantsAsync(filter);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error get merchants: {ex.Message}");
            }
        }

        [HttpGet("export-csv")]
        public async Task<IActionResult> ExportToCsv()
        {
            try
            {
                var csvStream = await merchantService.ExportToCsvAsync();
                return File(csvStream, "text/csv", $"merchants-{DateTimeOffset.UtcNow.Ticks}.csv");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error export  csv file: {ex.Message}");
            }
        }

    }
}
