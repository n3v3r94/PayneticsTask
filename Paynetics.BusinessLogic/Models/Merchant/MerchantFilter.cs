namespace Paynetics.BusinessLogic.Models
{
    public class MerchantFilter
    {
        public string? Name { get; set; }
        public DateTimeOffset? StartBoardingDate { get; set; }
        public DateTimeOffset? EndBoardingDate { get; set; }
        public string? Url { get; set; }
        public string? Country { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
