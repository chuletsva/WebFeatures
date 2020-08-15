using AutoMapper;
using Domian.Entities.Products;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.DataAccess;

namespace Application.Features.ProductReviews.CreateProductReview
{
    internal class CreateProductReviewCommandHandler : IRequestHandler<CreateProductReviewCommand, Guid>
    {
        private readonly IDbContext _db;
        private readonly IMapper _mapper;

        public CreateProductReviewCommandHandler(IDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateProductReviewCommand request, CancellationToken cancellationToken)
        {
            ProductReview review = _mapper.Map<ProductReview>(request);

            await _db.ProductReviews.AddAsync(review, cancellationToken);

            review.Events.Add(new ProductReviewCreated(review.Id));

            return review.Id;
        }
    }
}