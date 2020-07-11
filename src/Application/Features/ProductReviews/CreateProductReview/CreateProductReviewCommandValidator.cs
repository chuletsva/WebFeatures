using Application.Interfaces.DataAccess;
using FluentValidation;

namespace Application.Features.ProductReviews.CreateProductReview
{
    internal class CreateProductReviewCommandValidator : AbstractValidator<CreateProductReviewCommand>
    {
        public CreateProductReviewCommandValidator(IDbContext db)
        {
            RuleFor(x => x.ProductId)
                .MustAsync(async (x, t) => (await db.Products.FindAsync(x, t)) != null);

            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Comment).NotEmpty();
        }
    }
}