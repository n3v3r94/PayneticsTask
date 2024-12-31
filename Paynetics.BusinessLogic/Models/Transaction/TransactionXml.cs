namespace Paynetics.BusinessLogic.Models
{
    public class TransactionXml
    {
        public string? ExternalId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Amount? Amount { get; set; }
        public int Status { get; set; }
        public Debtor? Debtor { get; set; }
        public Beneficiary? Beneficiary { get; set; }
    }

}