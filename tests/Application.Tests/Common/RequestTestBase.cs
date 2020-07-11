using Application.Interfaces.Security;
using Application.Interfaces.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Domian.Entities.Accounts;
using Infrastructure;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Respawn;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.Common
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

            services.AddSingleton(Mock.Of<ICurrentUser>(x =>
                x.UserId == _currentUserId &&
                x.IsAuthenticated == true));
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

        protected async Task<Guid> LoginAsDefaultUserAsync()
        {
            return await LoginAsUserAsync("user@mail.com", "12345");
        }

        protected async Task<Guid> LoginAsUserAsync(string email, string password)
        {
            using IServiceScope scope = ServiceProvider.CreateScope();

            AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

            IPasswordHasher hasher = scope.ServiceProvider.GetService<IPasswordHasher>();

            var user = new User()
            {
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
        }

        private async Task CleanUpContextAsync()
        {
            using IServiceScope scope = ServiceProvider.CreateScope();

            AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

            DbConnection connection = context.Database.GetDbConnection();

            await connection.OpenAsync();

            await Checkpoint.Reset(connection);
        }

        private async Task SeedContextAsync()
        {
            await AddAsync(new Role() { Name = "users" });
        }

        public async Task DisposeAsync()
        {
        }
    }
}