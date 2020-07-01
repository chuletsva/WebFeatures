using MediatR;
using System;

namespace Application.Features.Products.DeleteProduct
{
    class ProductDeleted : INotification
    {
        public Guid Id { get; }

        public ProductDeleted(Guid id)
        {
            Id = id;
        }
    }
}