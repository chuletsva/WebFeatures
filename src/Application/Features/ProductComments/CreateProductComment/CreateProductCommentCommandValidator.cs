using Application.Common.Interfaces.DataAccess;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ProductComments.CreateProductComment
{
    internal class CreateProductCommentCommandValidator : AbstractValidator<CreateProductCommentCommand>
    {
        public CreateProductCommentCommandValidator(IDbContext db)
        {
            RuleFor(x => x.ProductId)
                .MustAsync((id, token) => db.Products.AnyAsync(x => x.Id == id, token));

            RuleFor(x => x.Body).NotEmpty();

            RuleFor(x => x.ParentCommentId)
                .MustAsync((id, token) => db.ProductComments.AnyAsync(x => x.Id == id, token))
                .When(x => x.ParentCommentId.HasValue);
        }
    }
}