using System.Linq;
using Application.Common.Models.Requests;

namespace Application.Features.Products.GetProducts
{
    public class GetProductsQuery : IQuery<IQueryable<ProductListDto>>
    {
    }
}
