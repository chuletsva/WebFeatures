using Application.Exceptions;
using Application.Infrastructure.Requests;
using MediatR;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Behaviours
{
    class ODataBehaviour<TRequest, TElement> : IPipelineBehavior<TRequest, IQueryable<TElement>>
        where TRequest : IODataQuery<TElement>
    {
        public async Task<IQueryable<TElement>> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<IQueryable<TElement>> next)
        {
            IQueryable<TElement> elements = await next();

            string query = GetQueryString(request);

            try
            {
                return elements;
            }
            catch
            {
                throw new ODataExeption("Error while applying odata");
            }
        }

        private string GetQueryString(TRequest request)
        {
            var sb = new StringBuilder();

            if (request.Filter != null)
            {
                sb.Append($"$filter={request.Filter}");
            }

            if (request.OrderBy != null)
            {
                sb.Append($"$orderby={request.OrderBy}");
            }

            if (request.Top.HasValue)
            {
                if (request.Top < 1)
                {
                    throw new ODataExeption("Top cannot be less than 1");
                }

                sb.Append($"$top={request.Top}");
            }

            if (request.Skip.HasValue)
            {
                if (request.Skip < 1)
                {
                    throw new ODataExeption("Skip cannot be less than 1");
                }

                sb.Append($"$skip={request.Top}");
            }

            return sb.ToString();
        }
    }
}