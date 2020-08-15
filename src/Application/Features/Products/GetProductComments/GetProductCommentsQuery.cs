using System;
using System.Linq;
using Application.Common.Models.Requests;

namespace Application.Features.Products.GetProductComments
{
    /// <summary>
    /// Получить комментарии к товару
    /// </summary>
    public class GetProductCommentsQuery : IQuery<IQueryable<ProductCommentInfoDto>>
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid ProductId { get; set; }
    }
}