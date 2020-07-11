using MediatR;
using System;

namespace Application.Features.ProductComments.CreateProductComment
{
    internal class ProductCommentCreated : INotification
    {
        public Guid Id { get; }

        public ProductCommentCreated(Guid id)
        {
            Id = id;
        }
    }
}
