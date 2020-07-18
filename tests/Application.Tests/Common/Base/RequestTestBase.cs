using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Constants;
using Application.Interfaces.Services;
using Application.Tests.Common.Helpers;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoFixture;
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
                currentUserService.SetupGet(x => x.IsAuthenticated).Returns(() => _currentUserId != default);   
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
            return await LoginAsync("user@mail.com");
        }

        protected static async Task<Guid> LoginAsync(string email)
        {
            using IServiceScope scope = ServiceProvider.CreateScope();

            AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

            User user = await context.Users.SingleOrDefaultAsync(x => x.Email == email) ??
                throw new InvalidOperationException("User doesn't exist");
            
            _currentUserId = user.Id;

            return _currentUserId;
        }

        public async Task InitializeAsync()
        {
            using IServiceScope scope = ServiceProvider.CreateScope();

            AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();
            
            await CleanUpContextAsync(context);

            await SeedContextAsync(context);

            // Temporary login for applying autocomplete logic before saving changes in context
            _currentUserId = UserId;
            
            await context.SaveChangesAsync();

            _currentUserId = default;
        }

        private async Task CleanUpContextAsync(AppDbContext context)
        {
            DbConnection connection = context.Database.GetDbConnection();

            await connection.OpenAsync();

            await Checkpoint.Reset(connection);
        }

        private async Task SeedContextAsync(AppDbContext context)
        {
            var fixture = new Fixture();
            {
                fixture.Customizations.Add(new EntitySpecimenBuilder());
            }

            Role role = fixture.Build<Role>()
               .With(x => x.Name, AuthorizationConstants.Roles.Users)
               .Create();
            {
                await context.AddAsync(role);
            }

            User user = fixture.Build<User>()
               .With(x => x.Email, "user@mail.com")
               .Create();
            {
                await context.AddAsync(user);

                UserId = user.Id;
            }

            Brand brand = fixture.Create<Brand>();
            {
                await context.AddAsync(brand);

                BrandId = brand.Id;
            }

            Category category = fixture.Create<Category>();
            {
                await context.AddAsync(category);

                CategoryId = category.Id;
            }

            Country country = fixture.Create<Country>();
            {
                await context.AddAsync(country);

                CountryId = country.Id;
            }

            City city = fixture.Build<City>()
               .With(x => x.CountryId, country.Id)
               .Create();
            {
                await context.AddAsync(city);

                CityId = city.Id;
            }

            Manufacturer manufacturer = fixture.Build<Manufacturer>()
               .With(x => x.HeadOffice, () =>
                    fixture.Build<Address>()
                       .With(x => x.CityId, CityId)
                       .Create())
               .Create();
            {
                await context.AddAsync(manufacturer);

                ManufacturerId = manufacturer.Id;
            }

            Currency currency = fixture.Create<Currency>();
            {
                await context.AddAsync(currency);

                CurrencyId = currency.Id;
            }

            Product product = fixture.Build<Product>()
               .With(x => x.Price, () =>
                    fixture.Build<Money>()
                       .With(x => x.CurrencyId, CurrencyId)
                       .Create())
               .With(x => x.BrandId, BrandId)
               .With(x => x.CategoryId, CategoryId)
               .With(x => x.ManufacturerId, ManufacturerId)
               .Create();
            {
                await context.AddAsync(product);

                ProductId = product.Id;
            }
        }

        protected Guid BrandId { get; private set; }      
        protected Guid CategoryId { get; private set; }      
        protected Guid CountryId { get; private set; }     
        protected Guid CityId { get; private set; }   
        protected Guid ManufacturerId { get; private set; }    
        protected Guid CurrencyId { get; private set; }
        protected Guid ProductId { get; private set; }    
        protected Guid UserId { get; private set; }

        public async Task DisposeAsync()
        {
        }
    }
}