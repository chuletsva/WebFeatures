using Application.Infrastructure.Requests;
using System;

namespace Application.Features.Products.GetProduct
{
    /// <summary>
    /// Получить товар
    /// </summary>
    public class GetProductQuery : IQuery<ProductInfoDto>
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid Id { get; set; }
    }
}
