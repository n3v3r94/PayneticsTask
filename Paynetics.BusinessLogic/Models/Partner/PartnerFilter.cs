namespace Paynetics.BusinessLogic.Models
{
    public class PartnerFilter
    {
        public string? Name { get; set; }
        public int PageNumber { get; set; } = 1; 
        public int PageSize { get; set; } = 10;
    }
}
