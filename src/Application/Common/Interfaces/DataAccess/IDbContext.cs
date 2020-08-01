using System.Threading;
using System.Threading.Tasks;
using Domian.Entities;
using Domian.Entities.Accounts;
using Domian.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces.DataAccess
{
    public interface IDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        DbSet<UserRole> UserRoles { get; }
        DbSet<File> Files { get; }
        DbSet<Product> Products { get; }
        DbSet<ProductPicture> ProductPictures { get; }
        DbSet<ProductReview> ProductReviews { get; }
        DbSet<ProductComment> ProductComments { get; }
        DbSet<Manufacturer> Manufacturers { get; }
        DbSet<Brand> Brands { get; }
        DbSet<Category> Categories { get; }
        DbSet<Shipper> Shippers { get; }
        DbSet<City> Cities { get; }
        DbSet<Country> Countries { get; }
        DbSet<Currency> Currencies { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}