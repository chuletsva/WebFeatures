﻿using Application.Interfaces.DataAccess;
using FluentValidation;

namespace Application.Features.Products.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator(IDbContext db)
        {
            RuleFor(x => x.Id)
                .MustAsync(async (x, t) => (await db.Products.FindAsync(x, t)) != null);

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.PriceAmount).Must(x => x >= 0);

            RuleFor(x => x.PriceCurrencyId)
                .MustAsync(async (x, t) => (await db.Currencies.FindAsync(x, t)) != null);

            RuleFor(x => x.ManufacturerId)
                .MustAsync(async (x, t) => (await db.Manufacturers.FindAsync(x, t)) != null);

            RuleFor(x => x.CategoryId)
                .MustAsync(async (x, t) => (await db.Categories.FindAsync(x, t)) != null)
                .When(x => x.CategoryId.HasValue);

            RuleFor(x => x.BrandId)
                .MustAsync(async (x, t) => (await db.Brands.FindAsync(x, t)) != null);
        }
    }
}