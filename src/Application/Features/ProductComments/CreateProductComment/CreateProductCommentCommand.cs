using System;
using Application.Models.Requests;

namespace Application.Features.ProductComments.CreateProductComment
{
    /// <summary>
    /// Создать комментарий к товару
    /// </summary>
    public class CreateProductCommentCommand : CommandBase<Guid>, IRequireAuthorization
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Индентификатор родительского комментария
        /// </summary>
        public Guid? ParentCommentId { get; set; }
    }
}