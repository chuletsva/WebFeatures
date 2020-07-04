using Domian.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configurations
{
    class ShipperConfiguration : IEntityTypeConfiguration<Shipper>
    {
        public void Configure(EntityTypeBuilder<Shipper> builder)
        {
            builder.Property(x => x.OrganizationName).IsRequired();

            builder.OwnsOne(x => x.HeadOffice, navigation =>
            {
                navigation.Property(x => x.StreetName).IsRequired();

                navigation.Property(x => x.ZipCode).IsRequired();

                navigation.HasOne(x => x.City)
                    .WithMany()
                    .HasForeignKey(x => x.CityId)
                    .IsRequired();
            });
        }
    }
}
