using System.Collections;

namespace WebApi.Pagination
{
    public class PaginationResponse
    {
        public int Total { get; set; }
        public int PerPage { get; set; }
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
        public ICollection Items { get; set; }
    }
}