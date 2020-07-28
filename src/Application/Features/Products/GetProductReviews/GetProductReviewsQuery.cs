using System;
using System.Linq;
using Application.Models.Requests;

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