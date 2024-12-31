namespace Paynetics.Data.Entities
{
    /// <summary>
    /// Beneficiary
    /// </summary>
    public class Merchant
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public DateTimeOffset BoardingDate { get; set; }
        public string? Url { get; set; }
        public string?  Country { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public Partner? Partner { get; set; }
        public long PartnerId { get; set; }
        public HashSet<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
    }
}
