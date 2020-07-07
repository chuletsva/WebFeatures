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
using Respawn;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.Common
{
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
                DbAdapter = DbAdapter.Postgres,
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

        protected static async Task<TEntity> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            var db = scope.ServiceProvider.GetService<AppDbContext>();

            return await db.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        protected static async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            var db = scope.ServiceProvider.GetService<AppDbContext>();

            await db.AddAsync(entity);

            await db.SaveChangesAsync();
        }

        public async Task InitializeAsync()
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

            DbConnection connection = context.Database.GetDbConnection();

            await connection.OpenAsync();

            await _checkpoint.Reset(connection);

            await SeedAsync(context);
        }

        private async Task SeedAsync(AppDbContext context)
        {
            await context.Roles.AddAsync(new Role() { Name = "users" });

            await context.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
        }
    }
}