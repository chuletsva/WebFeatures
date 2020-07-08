using Application.Interfaces.Security;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Domian.Entities.Accounts;
using Infrastructure;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.Common
{
    [Collection("Integration")]
    public class RequestTestBase : IAsyncLifetime
    {
        private static IServiceProvider _serviceProvider;
        private static Checkpoint _checkpoint;

        static RequestTestBase()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>()
                {
                    { "ASPNETCORE_ENVIRONMENT", "Testing" },
                    { "ConnectionStrings:Testing", "server=localhost;port=5432;database=webfeatures_test_db;username=postgres;password=postgres;"}
                })
                .Build();

            var services = new ServiceCollection();

            services.AddLogging();

            services.AddApplication();
            services.AddInfrastructure(configuration);

            var containerBuilder = new ContainerBuilder();

            containerBuilder.Populate(services);

            _serviceProvider = new AutofacServiceProvider(containerBuilder.Build());

            _checkpoint = new Checkpoint()
            {
                SchemasToInclude = new[] { "public" },
                TablesToIgnore = new[] { "__EFMigrationsHistory" },
                DbAdapter = DbAdapter.Postgres
            };

            EnsureDatabase();
        }

        private static void EnsureDatabase()
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            using AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

            context.Database.EnsureDeleted();

            context.Database.Migrate();
        }

        protected static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();

            return await mediator.Send(request);
        }

        protected static async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

            await context.AddAsync(entity);

            await context.SaveChangesAsync();
        }

        protected static async Task<TEntity> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

            return await context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        protected async Task<Guid> LoginAsDefaultUserAsync()
        {
            return await LoginAsUserAsync("user@mail.com", "12345");
        }

        protected async Task<Guid> LoginAsUserAsync(string email, string password)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

            IPasswordHasher hasher = scope.ServiceProvider.GetService<IPasswordHasher>();

            var user = new User()
            {
                Email = email,
                PasswordHash = hasher.ComputeHash(password)
            };

            await context.AddAsync(user);

            await context.SaveChangesAsync();

            return user.Id;
        }

        public async Task InitializeAsync()
        {
            await CleanUpContextAsync();

            await SeedContextAsync();
        }

        private async Task CleanUpContextAsync()
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

            DbConnection connection = context.Database.GetDbConnection();

            await connection.OpenAsync();

            await _checkpoint.Reset(connection);
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