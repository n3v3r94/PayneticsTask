using Paynetics.BusinessLogic.Models;
using Paynetics.BusinessLogic.Models.Transaction;
using Paynetics.Data.Entities;

namespace Paynetics.BusinessLogic.Services
{
    public interface ITransactionService
    {
        Task ProceedBatchAsync(TransactionBatch batch);
        Task<MemoryStream> ExportToCsvAsync();
        Task<PaginatedResponse<Transaction>> GetTransactionsAsync(TransactionFilter filter);
    }
}
