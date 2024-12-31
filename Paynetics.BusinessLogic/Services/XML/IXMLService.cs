using Paynetics.BusinessLogic.Models;

namespace Paynetics.BusinessLogic.Services.XML
{
    public interface IXMLService
    {
        IAsyncEnumerable<TransactionBatch> ReadTransactionBatch(Stream fileStream, int batchCount = 25000);
    }
}
