using MediatR;
using System;

namespace Application.Features.Products.UpdateProduct
{
    internal class ProductUpdated : INotification
    {
        public Guid Id { get; }

        public ProductUpdated(Guid id)
        {
            Id = id;
        }
    }
}
