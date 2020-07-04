using System;

namespace Application.Features.Products.GetProductComments
{
    /// <summary>
    /// Информация о комментарии
    /// </summary>
    public class ProductCommentInfoDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Body { get; }

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
        /// Идентификатор родительского комментария
        /// </summary>
        public Guid? ParentCommentId { get; }
    }
}