using Microsoft.AspNetCore.Mvc;

namespace WebApi.Pagination
{
    public class PaginationRequest
    {
        [BindProperty(Name = "$filter", SupportsGet = true)]
        public string Where { get; set; }

        [BindProperty(Name = "$top", SupportsGet = true)]
        public int Top { get; set; }

        [BindProperty(Name = "$skip", SupportsGet = true)]
        public int Skip { get; set; }

        [BindProperty(Name = "$orderby", SupportsGet = true)]
        public string OrderBy { get; set; }
    }
}