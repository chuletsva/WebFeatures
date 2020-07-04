using Domian.Enums;
using System;

namespace Application.Features.Products.GetProductReviews
{
    /// <summary>
    /// Инофрмация оо обзоре на товар
    /// </summary>
    public class ProductReviewInfoDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Автор
        /// </summary>
        public Guid CreatedById { get; set; }
        public string CreatedByName { get; set; }

        /// <summary>
        /// Пользовательская оценка
        /// </summary>
        public ProductRating Rating { get; set; }

        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid ProductId { get; set; }
    }
}