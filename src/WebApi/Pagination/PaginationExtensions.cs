using LinqToQuerystring;
using System.Linq;

namespace WebApi.Pagination
{
    internal static class PaginationExtensions
    {
        public static PaginationResponse ApplyPagination<T>(this IQueryable<T> queryable, PaginationRequest request)
        {
            var response = new PaginationResponse();

            if (request.Top < 1 || request.Skip < 0)
            {
                throw new PaginationException("");
            }

            if (!string.IsNullOrWhiteSpace(request.Where))
            {
                queryable = queryable.ApplyQuery($"$filter={request.Where}");
            }

            response.Total = queryable.Count();
            response.PerPage = request.Top;

            queryable = queryable.Skip(request.Skip).Take(request.Top);

            response.CurrentPage = request.Skip / request.Top + 1;
            response.LastPage = (response.Total - 1) / request.Top + 1;

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                queryable = queryable.ApplyQuery($"$orderby={request.OrderBy}");
            }

            response.Items = queryable.ToList();

            return response;
        }

        private static IQueryable<T> ApplyQuery<T>(this IQueryable<T> queryable, string query)
        {
            try
            {
                return queryable.LinqToQuerystring(query);
            }
            catch
            {
                throw new PaginationException("");
            }
        }
    }
}
