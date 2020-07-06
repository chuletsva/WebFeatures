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
                    { "ConnectionStrings:Testing", "server=localhost;port=5432;database=webfeatures_test_db;username=postgres;password=postgres"}
                })
                .Build();

            var services = new ServiceCollection();

            services.AddApplication();
            services.AddInfrastructure(configuration);

            _serviceProvider = services.BuildServiceProvider();

            _checkpoint = new Checkpoint()
            {
                DbAdapter = DbAdapter.Postgres,
                TablesToIgnore = new[] { "__EFMigrationsHistory" }
            };

            EnsureDatabase();
        }

        private static void EnsureDatabase()
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            using AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

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

            return await db.FindAsync<TEntity>(predicate);
        }

        protected static async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            var db = scope.ServiceProvider.GetService<AppDbContext>();

            await db.AddAsync(entity);
        }

        public async Task InitializeAsync()
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            AppDbContext db = scope.ServiceProvider.GetService<AppDbContext>();

            DbConnection connection = db.Database.GetDbConnection();

            await connection.OpenAsync();

            await _checkpoint.Reset(connection);
        }

        public async Task DisposeAsync()
        {
        }
    }
}