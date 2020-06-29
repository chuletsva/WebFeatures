using Application.Interfaces.Common;
using Application.Interfaces.DataAccess;
using Domian.Common;
using Domian.Entities;
using Domian.Entities.Accounts;
using Domian.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess
{
    class BaseDbContext : DbContext, IDbContext
    {
        private readonly IDateTime _dateTime;

        protected BaseDbContext(IDateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (EntityEntry entiry in ChangeTracker.Entries())
            {
                if (entiry.State == EntityState.Added)
                {
                    if (entiry.Entity is IHasCreateDate cd)
                    {
                        cd.CreateDate = _dateTime.Now;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<User> Users { get; }
        public DbSet<Role> Roles { get; }
        public DbSet<UserRole> UserRoles { get; }
        public DbSet<File> Files { get; }
        public DbSet<Product> Products { get; }
        public DbSet<ProductPicture> ProductPictures { get; }
        public DbSet<ProductReview> ProductReviews { get; }
        public DbSet<ProductComment> ProductComments { get; }
        public DbSet<Manufacturer> Manufacturers { get; }
        public DbSet<Brand> Brands { get; }
        public DbSet<Category> Categories { get; }
        public DbSet<Shipper> Shippers { get; }
        public DbSet<City> Cities { get; }
        public DbSet<Country> Countries { get; }
    }
}