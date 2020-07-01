using Domian.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configurations
{
    class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(x => x.CreatedBy)
                .WithOne()
                .HasForeignKey<Product>(x => x.CreatedById)
                .IsRequired();

            builder.HasOne(x => x.UpdatedBy)
                .WithOne()
                .HasForeignKey<Product>(x => x.UpdatedById)
                .IsRequired();

            builder.OwnsOne(x => x.Price, navigation =>
            {
                navigation.HasOne(x => x.Currency)
                    .WithMany()
                    .HasForeignKey(x => x.CurrencyId)
                    .IsRequired();
            });
        }
    }
}