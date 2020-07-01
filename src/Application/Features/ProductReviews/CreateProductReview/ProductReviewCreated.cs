using MediatR;
using System;

namespace Application.Features.ProductReviews.CreateProductReview
{
    class ProductReviewCreated : INotification
    {
        public Guid Id { get; }

        public ProductReviewCreated(Guid id)
        {
            Id = id;
        }
    }
}