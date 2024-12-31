using Paynetics.Data.Enum;

namespace Paynetics.BusinessLogic.Models.Transaction
{
    public class TransactionFilter
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string? Direction { get; set; }
        public string? Currency { get; set; }
        public string? DebtorIban { get; set; }
        public string? BeneficiaryIban { get; set; }
        public TransactionStatus? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
