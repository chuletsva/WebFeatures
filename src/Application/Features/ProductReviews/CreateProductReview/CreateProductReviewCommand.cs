using Application.Infrastructure.Requests;
using Domian.Enums;
using System;

namespace Application.Features.ProductReviews.CreateProductReview
{
    /// <summary>
    /// Создать обзор на товар
    /// </summary>
    public class CreateProductReviewCommand : CommandBase<Guid>
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Пользовательская оценка
        /// </summary>
        public ProductRating Rating { get; set; }
    }
}