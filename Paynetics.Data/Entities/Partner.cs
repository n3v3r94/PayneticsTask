namespace Paynetics.Data.Entities
{
    /// <summary>
    /// Debtor
    /// </summary>
    public class Partner
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public HashSet<Merchant> Merchants { get; set; } = new HashSet<Merchant>();
    }
}
