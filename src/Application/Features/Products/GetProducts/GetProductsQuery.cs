using Application.Infrastructure.Requests;
using System.Linq;

namespace Application.Features.Products.GetProducts
{
    public class GetProductsQuery : IQuery<IQueryable<ProductListDto>>
    {
    }
}
