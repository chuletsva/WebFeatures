using Application.Infrastructure.Requests;
using System;
using System.Linq;

namespace Application.Features.Products.GetProductReviews
{
    /// <summary>
    /// Получить обзоры на товар
    /// </summary>
    public class GetProductReviewsQuery : IQuery<IQueryable<ProductReviewInfoDto>>
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid ProductId { get; set; }
    }
}