using Microsoft.AspNetCore.Mvc;
using Paynetics.BusinessLogic.Models.Transaction;
using Paynetics.BusinessLogic.Services;
using Paynetics.BusinessLogic.Services.XML;

namespace Paynetics.API.Controllers
{

    public class TransactionController : APIControllerBase
    {
        private readonly IXMLService xmlService;
        private readonly ITransactionService transactionService;

        public TransactionController(IXMLService xmlService, ITransactionService transactionService)
        {
            this.xmlService = xmlService;
            this.transactionService = transactionService;
        }

        [HttpGet("filter-transaction")]
        public async Task<IActionResult> FilterTransaction([FromQuery] TransactionFilter filter)
        {
            try
            {
                var data = await transactionService.GetTransactionsAsync(filter);
                return Ok(data);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error get transaction: {ex.Message}");
            }
        }

        [HttpGet("generate-xml")]
        public async Task<IActionResult> GenerateXmlFile()
        {
            string xmlFilePath = TransactionGenerator.GenerateXmlForTransactions(1_000_000, $"transaction-{DateTime.UtcNow.Ticks}.xml");

            return Ok($"The file is generated in {xmlFilePath}");
        }

        [HttpPost("import-xml")]
        public async Task<IActionResult> ImportXML(IFormFile transactionsFile)
        {
            if (transactionsFile == null || transactionsFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                using (var stream = transactionsFile.OpenReadStream())
                {
                    await foreach (var batch in xmlService.ReadTransactionBatch(stream))
                    {
                        await transactionService.ProceedBatchAsync(batch);
                    }
                }

                return Ok($"Successfully imported  transaction.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error importing the file: {ex.Message}");
            }
        }

        [HttpGet("export-csv")]
        public async Task<IActionResult> ExportToCsv()
        {
            try
            {
                var csvStream = await transactionService.ExportToCsvAsync();
                return File(csvStream, "text/csv", $"transaction-{DateTimeOffset.UtcNow.Ticks}.csv");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error export  csv file: {ex.Message}");
            }
        }

    }
}
