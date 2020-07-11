using Application.Interfaces.DataAccess;
using FluentValidation;

namespace Application.Features.ProductComments.CreateProductComment
{
    internal class CreateProductCommentCommandValidator : AbstractValidator<CreateProductCommentCommand>
    {
        public CreateProductCommentCommandValidator(IDbContext db)
        {
            RuleFor(x => x.ProductId)
                .MustAsync(async (x, t) => (await db.Products.FindAsync(x, t)) != null);

            RuleFor(x => x.Body).NotEmpty();

            RuleFor(x => x.ParentCommentId)
                .MustAsync(async (x, t) => (await db.ProductComments.FindAsync(x, t)) != null)
                .When(x => x.ParentCommentId.HasValue);
        }
    }
}