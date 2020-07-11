using MediatR;
using System;

namespace Application.Features.Products.CreateProduct
{
    internal class ProductCreated : INotification
    {
        public Guid Id { get; }

        public ProductCreated(Guid id)
        {
            Id = id;
        }
    }
}