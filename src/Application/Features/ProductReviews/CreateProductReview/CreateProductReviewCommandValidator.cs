using Application.Interfaces.DataAccess;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ProductReviews.CreateProductReview
{
    internal class CreateProductReviewCommandValidator : AbstractValidator<CreateProductReviewCommand>
    {
        public CreateProductReviewCommandValidator(IDbContext db)
        {
            RuleFor(x => x.ProductId)
                .MustAsync((id, token) => db.Products.AnyAsync(x => x.Id == id, token));

            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Comment).NotEmpty();
        }
    }
}