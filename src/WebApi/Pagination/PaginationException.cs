using System;

namespace WebApi.Pagination
{
    class PaginationException : Exception
    {
        public PaginationException(string message) : base(message)
        {
        }
    }
}
