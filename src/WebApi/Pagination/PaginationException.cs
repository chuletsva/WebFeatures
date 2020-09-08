using System;

namespace WebApi.Pagination
{
    internal class PaginationException : Exception
    {
        public PaginationException(string message) : base(message)
        {
        }
    }
}
