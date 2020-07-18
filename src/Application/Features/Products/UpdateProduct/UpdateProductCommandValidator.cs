using System;
using Application.Interfaces.DataAccess;
using FluentValidation;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.UpdateProduct
{
    internal class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        private static readonly string[] PictureExtensions = {".jpg", ".jpeg"};
        
        private readonly IDbContext _db;
        
        public UpdateProductCommandValidator(IDbContext db)
        {
            _db = db;
            
            RuleFor(x => x.Id)
                .MustAsync(async (id, token) => await db.Products.FindAsync(new object[]{id}, token) != null);

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.PriceAmount).Must(x => x >= 0);

            RuleFor(x => x.PriceCurrencyId)
                .MustAsync((id, token) => db.Currencies.AnyAsync(x => x.Id == id, token));

            RuleFor(x => x.PictureId)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .MustAsync((id, token) => db.Files.AnyAsync(x => x.Id == id, token))
               .MustAsync(HaveValidPictureExtension)
               .WithMessage($"Allowed picture extensions: {string.Join(", ", PictureExtensions)}")
               .When(x => x.PictureId.HasValue);

            RuleFor(x => x.ManufacturerId)
                .MustAsync((id, token) => db.Manufacturers.AnyAsync(x => x.Id == id, token));

            RuleFor(x => x.CategoryId)
                .MustAsync((id, token) => db.Categories.AnyAsync(x => x.Id == id, token))
                .When(x => x.CategoryId.HasValue);

            RuleFor(x => x.BrandId)
                .MustAsync((id, token) => db.Brands.AnyAsync(x => x.Id == id, token));
        }

        private async Task<bool> HaveValidPictureExtension(Guid? id, CancellationToken cancellationToken)
        {
            Domian.Entities.File picture = await _db.Files.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (picture == null) return false;
                    
            string extension = Path.GetExtension(picture.Name);
                    
            return PictureExtensions.Contains(extension);
        }
    }
}