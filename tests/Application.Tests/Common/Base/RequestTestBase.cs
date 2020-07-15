using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Application.Constants;
using Application.Interfaces.DataAccess;
using Application.Interfaces.Security;
using Application.Interfaces.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoFixture;
using AutoFixture.Kernel;
using AutoMapper.Internal;
using Domian.Common;
using Domian.Entities;
using Domian.Entities.Accounts;
using Domian.Entities.Products;
using Domian.ValueObjects;
using Infrastructure;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Respawn;
using Xunit;

namespace Application.Tests.Common.Base
{
    [Collection("Integration")]
    public class RequestTestBase : IAsyncLifetime
    {
        private static readonly IServiceProvider ServiceProvider;
        private static readonly Checkpoint Checkpoint;
        
        private static Guid _currentUserId;

        static RequestTestBase()
        {
            IConfiguration configuration = CreateConfiguration();

            var services = new ServiceCollection();

            AddServices(services, configuration);

            var serviceProvider = CreateServiceProvider(services);

            EnsureDatabase(serviceProvider);

            ServiceProvider = serviceProvider;

            Checkpoint = new Checkpoint()
            {
                SchemasToInclude = new[] { "public" },
                TablesToIgnore = new[] { "__EFMigrationsHistory" },
                DbAdapter = DbAdapter.Postgres
            };
        }

        private static IConfiguration CreateConfiguration()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>()
                {
                    { "ASPNETCORE_ENVIRONMENT", "Testing" },
                    { "ConnectionStrings:Testing", "server=localhost;port=5432;database=webfeatures_test_db;username=postgres;password=postgres;"}
                })
                .Build();

            return configuration;
        }

        private static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging();
            services.AddApplication();          
            services.AddInfrastructure(configuration);

            var currentUserServiceDescription = services.First(x => x.ServiceType == typeof(ICurrentUser));

            services.Remove(currentUserServiceDescription);

            var currentUserService = new Mock<ICurrentUser>();
            {
                currentUserService.SetupGet(x => x.UserId).Returns(() => _currentUserId);
                currentUserService.SetupGet(x => x.IsAuthenticated).Returns(() => _currentUserId != Guid.Empty);   
            };
            
            services.AddSingleton(currentUserService.Object);
        }

        private static IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.Populate(services);

            return new AutofacServiceProvider(containerBuilder.Build());
        }

        private static void EnsureDatabase(IServiceProvider serviceProvider)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            using AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

            context.Database.EnsureDeleted();

            context.Database.Migrate();
        }

        protected static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using IServiceScope scope = ServiceProvider.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();
            
            return await mediator.Send(request);
        }

        protected static async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using IServiceScope scope = ServiceProvider.CreateScope();

            AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

            await context.AddAsync(entity);

            await context.SaveChangesAsync();
        }

        protected static async Task<TEntity> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            using IServiceScope scope = ServiceProvider.CreateScope();

            AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

            return await context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        protected static async Task<Guid> LoginAsDefaultUserAsync()
        {
            return await LoginAsync("user@mail.com", "12345");
        }

        protected static async Task<Guid> LoginAsync(string email, string password)
        {
            using IServiceScope scope = ServiceProvider.CreateScope();

            AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

            IPasswordHasher hasher = scope.ServiceProvider.GetService<IPasswordHasher>();

            var user = new User()
            {
                Name = email,
                Email = email,
                PasswordHash = hasher.ComputeHash(password)
            };

            await context.AddAsync(user);

            await context.SaveChangesAsync();

            _currentUserId = user.Id;

            return _currentUserId;
        }

        public async Task InitializeAsync()
        {
            await CleanUpContextAsync();

            await SeedContextAsync();
            
            TestData.Init(ServiceProvider);
        }

        private static async Task CleanUpContextAsync()
        {
            using IServiceScope scope = ServiceProvider.CreateScope();

            AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

            DbConnection connection = context.Database.GetDbConnection();

            await connection.OpenAsync();

            await Checkpoint.Reset(connection);
        }

        private static async Task SeedContextAsync()
        {
            await AddAsync(new Role()
            {
                Name = AuthorizationConstants.Roles.Users
            });
        }

        public async Task DisposeAsync()
        {
        }
    }

    internal static class TestData
    {
        public static User[] Users { get; private set; }
        public static Brand[] Brands { get; private set; }
        public static Product[] Products { get; private set; }
        public static Manufacturer[] Manufacturers { get; private set; }
        public static City[] Cities { get; private set; }
        public static Category[] Categories { get; private set; }
        public static Country[] Countries { get; private set; }
        public static Currency[] Currencies { get; private set; }
        public static Role[] Roles { get; private set; }
        public static File[] Files { get; private set; }
        
        public static void Init(IServiceProvider services)
        {
            using IServiceScope scope = services.CreateScope();
            
            IPasswordHasher hasher = scope.ServiceProvider.GetService<IPasswordHasher>();
            
            Users = new[]
            {
                new User()
                {
                    Id = new Guid("f9fb9878-6236-44d0-9941-21417ec8d5f6"),
                    Name = "user",
                    Email = "user@mail.com",
                    PasswordHash = hasher.ComputeHash("12345")
                }
            };
            
            Roles = new[]
            {
                new Role()
                {
                    Name = AuthorizationConstants.Roles.Users,
                    Description = "Пользователи"
                }
            };

            Brands = new[]
            {
                new Brand()
                {
                    Id = new Guid("873d861e-14e1-4757-91f4-cae6beba4010"),
                    Name = "name"
                }
            };
                       
            Manufacturers = new[]
            {
                new Manufacturer()
                {
                    Id = new Guid("0ea02742-3566-416f-94e5-bc9d878769f3"),
                    OrganizationName = "manufacturer",
                    HeadOffice = new Address()
                    {
                        CityId = new Guid("685e8905-bad5-4767-ac2c-2bae7ead13be"),
                        StreetName = "streetname",
                        ZipCode = "12345"
                    }
                }
            };
            
            Countries = new[]
            {
                new Country()
                {
                    Id = new Guid("46bd74a1-e141-45e2-aaec-ba3616d95819"),
                    Continent = "continent",
                    Name = "country"
                }
            };
          
            Cities = new[]
            {
                new City()
                {
                    Id = new Guid("685e8905-bad5-4767-ac2c-2bae7ead13be"),
                    CountryId = new Guid("46bd74a1-e141-45e2-aaec-ba3616d95819"),
                    Name = "city"
                }
            };

            Currencies = new[]
            {
                new Currency()
                {
                    Id = new Guid("d39e9efe-07af-40d7-bb5e-9904f4bc0fc2"),
                    Code = "RUB"
                }
            };

            Categories = new[]
            {
                new Category
                {
                    Id = new Guid("5805119e-613f-433d-a5d7-ecf9f553a984"),
                    Name = "Category",
                }
            };
            
            Products = new[]
            {
                new Product()
                {
                    Name = "product",
                    Description = "description",
                    CreatedAt = DateTime.UtcNow,
                    Price = new Money()
                    {
                        Amount = 1
                    }
                }
            };

            Files = new[]
            {
                new File
                {
                    Id = new Guid("e6bc62a3-8dce-460b-8c23-11d38eace343"),
                    Name = "Content",
                    ContentType = "ContentType",
                    Content = new byte[] {1, 2, 3}
                }
            };
            
            Cities[0].CountryId = Countries[0].Id;
            
            Products[0].PictureId = Files[0].Id;
            Products[0].ManufacturerId = Manufacturers[0].Id;
            Products[0].CategoryId = Categories[0].Id;
            Products[0].CreatedById = Users[0].Id;
            Products[0].Price.CurrencyId = Currencies[0].Id;
        }

        private class EntitySpecimenBuilder : ISpecimenBuilder
        {
            public object Create(object request, ISpecimenContext context)
            {
                if (!(request is PropertyInfo property) ||
                    property.PropertyType.IsNullableType() ||
                    property.PropertyType.IsSubclassOf(typeof(Entity)) ||
                    property.PropertyType.IsCollectionType() ||
                    property.PropertyType == typeof(Guid))
                {
                    return new NoSpecimen();
                }

                return context.Resolve(request);
            }
        }
    }
}