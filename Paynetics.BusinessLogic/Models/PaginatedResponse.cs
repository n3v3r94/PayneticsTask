namespace Paynetics.BusinessLogic.Models
{
    public class PaginatedResponse<T>
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public List<T> Data { get; set; }
    }

}
