using MediatR;
using System;
using Application.Common.Models.Requests;

namespace Application.Features.Products.DeleteProduct
{
    /// <summary>
    /// Удалить товар
    /// </summary>
    public class DeleteProductCommand : CommandBase<Unit>, IRequireAuthorization
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid Id { get; set; }
    }
}
