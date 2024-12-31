using Paynetics.Data.Enum;

namespace Paynetics.Data.Entities
{
    public class Transaction
    {
        public long Id { get; set; }
        public string? ExternalId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public string? Direction { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? DebtorIban { get; set; }
        public string? BeneficiaryIban{ get; set; }
        public TransactionStatus? Status { get; set; }
        public Merchant? Merchant { get; set; }
        public long MerchantId { get; set; }

    }
}
