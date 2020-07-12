using Application.Interfaces.DataAccess;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.CreateProduct
{
    internal class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator(IDbContext db)
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.PriceAmount).Must(x => x >= 0);

            RuleFor(x => x.PriceCurrencyId)
                .MustAsync((id, token) => db.Currencies.AnyAsync(x => x.Id == id, token));

            RuleFor(x => x.ManufacturerId)
                .MustAsync((id, token) => db.Manufacturers.AnyAsync(x => x.Id == id, token));

            RuleFor(x => x.CategoryId)
                .MustAsync((id, token) => db.Categories.AnyAsync(x => x.Id == id, token))
                .When(x => x.CategoryId.HasValue);

            RuleFor(x => x.BrandId)
                .MustAsync((id, token) => db.Brands.AnyAsync(x => x.Id == id, token));
        }
    }
}