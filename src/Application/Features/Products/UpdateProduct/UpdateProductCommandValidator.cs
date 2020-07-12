using Application.Interfaces.DataAccess;
using FluentValidation;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.UpdateProduct
{
    internal class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator(IDbContext db)
        {
            RuleFor(x => x.Id)
                .MustAsync((id, token) => db.Products.AnyAsync(x => x.Id == id, token));

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.PriceAmount).Must(x => x >= 0);

            RuleFor(x => x.PriceCurrencyId)
                .MustAsync((id, token) => db.Currencies.AnyAsync(x => x.Id == id, token));

            RuleFor(x => x.PictureId)
                .MustAsync(async (x, t) =>
                {
                    Domian.Entities.File picture = await db.Files.FindAsync(x, t);

                    if (picture == null) return false;

                    return Path.GetExtension(picture.Name) == ".jpg";
                })
                .When(x => x.PictureId.HasValue);

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