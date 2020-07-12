using Application.Infrastructure.Requests;
using MediatR;
using System;

namespace Application.Features.Products.DeleteProduct
{
    /// <summary>
    /// Удалить товар
    /// </summary>
    public class DeleteProductCommand : CommandBase<Unit>, IAuthorization
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid Id { get; set; }
    }
}
