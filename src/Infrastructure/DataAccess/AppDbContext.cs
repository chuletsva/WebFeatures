using Application.Interfaces.DataAccess;
using Application.Interfaces.Services;
using Domian.Common;
using Domian.Entities;
using Domian.Entities.Accounts;
using Domian.Entities.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.DataAccess
{
    public class AppDbContext : DbContext, IDbContext
    {
        private readonly ICurrentUser _currentUser;
        private readonly IDateTime _dateTime;
        private readonly IMediator _mediator;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            ICurrentUser currentUser,
            IDateTime dateTime,
            IMediator mediator) : base(options)
        {
            _currentUser = currentUser;
            _dateTime = dateTime;
            _mediator = mediator;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            IgnoreEvents(modelBuilder);

            SetSoftDeleteFilter(modelBuilder);
        }

        private void IgnoreEvents(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!entityType.ClrType.IsSubclassOf(typeof(Entity))) continue;
                
                MethodInfo ignoreEvents = typeof(AppDbContext).GetMethod(
                        nameof(IgnoreEventsImpl),
                        BindingFlags.NonPublic | BindingFlags.Static)
                   .MakeGenericMethod(entityType.ClrType);

                ignoreEvents.Invoke(null, new object[] { modelBuilder });
            }
        }

        private static void IgnoreEventsImpl<T>(ModelBuilder modelBuilder) where T : Entity
        {
            modelBuilder.Entity<T>().Ignore(x => x.Events);
        }

        private void SetSoftDeleteFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetInterfaces().All(x => x != typeof(ISoftDelete))) continue;
                
                MethodInfo setFilter = typeof(AppDbContext).GetMethod(
                        nameof(SetSoftDeleteFilterImpl),
                        BindingFlags.NonPublic | BindingFlags.Static)
                   .MakeGenericMethod(entityType.ClrType);

                setFilter.Invoke(null, new object[] { modelBuilder });
            }
        }

        private static void SetSoftDeleteFilterImpl<T>(ModelBuilder modelBuilder) where T : class, ISoftDelete
        {
            modelBuilder.Entity<T>().HasQueryFilter(x => !x.IsDeleted);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditData();

            SetSoftDelete();

            await PublishEventsAsync(cancellationToken);

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetAuditData()
        {
            foreach (EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        {
                            entry.Entity.CreatedAt = _dateTime.Now;
                            entry.Entity.CreatedById = _currentUser.UserId;

                            break;
                        }

                    case EntityState.Modified:
                        {
                            entry.Entity.UpdatedAt = _dateTime.Now;
                            entry.Entity.UpdatedById = _currentUser.UserId;

                            break;
                        }
                }
            }
        }

        private void SetSoftDelete()
        {
            foreach (EntityEntry<ISoftDelete> entry in ChangeTracker.Entries<ISoftDelete>())
            {
                if (entry.State != EntityState.Deleted) continue;
                
                entry.State = EntityState.Modified;

                entry.Entity.IsDeleted = true;
            }
        }

        private async Task PublishEventsAsync(CancellationToken cancellationToken)
        {
            INotification[] notifications = ChangeTracker.Entries<Entity>()
                .SelectMany(x => x.Entity.Events)
                .ToArray();

            foreach (INotification notification in notifications)
            {
                await _mediator.Publish(notification, cancellationToken);
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Currency> Currencies { get; set; }
    }
}