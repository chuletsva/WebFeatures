using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Common.Interfaces.CommonServices;
using Application.Common.Interfaces.Security;
using Application.Tests.Common.Helpers;
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
using Xunit;

namespace Application.Tests.Common.Base
{
    [Collection("Requests")]
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
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration;
        }

        private static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterApplication();

            services.RegisterBackgroundJobs(configuration);
            services.RegisterCommonServices();
            services.RegisterDataAccess(configuration);
            services.RegisterLogging();
            services.RegisterSecurity();

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

        protected static async Task<List<TElement>> SendAsync<TElement>(IRequest<IQueryable<TElement>> request)
        {
            using IServiceScope scope = ServiceProvider.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();

            return (await mediator.Send(request)).ToList();
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
            return await LoginAsync("default@user");
        }

        protected static async Task<Guid> LoginAsync(string email)
        {
            using IServiceScope scope = ServiceProvider.CreateScope();

            AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

            User user = await context.Users.SingleOrDefaultAsync(x => x.Email == email) ??
                throw new Exception("User doesn't exist");

            _currentUserId = user.Id;

            return _currentUserId;
        }

        public async Task InitializeAsync()
        {
            using IServiceScope scope = ServiceProvider.CreateScope();

            var context = scope.ServiceProvider.GetService<AppDbContext>();

            await CleanUpContextAsync(context);

            var hasher = scope.ServiceProvider.GetService<IPasswordHasher>();

            await EntityTestData.SeedContextAsync(context, hasher);

            // Temporary login for applying autocomplete logic before saving changes in context
            _currentUserId = EntityTestData.UserId;

            await context.SaveChangesAsync();

            _currentUserId = default;
        }

        private static async Task CleanUpContextAsync(DbContext context)
        {
            DbConnection connection = context.Database.GetDbConnection();

            await connection.OpenAsync();

            await Checkpoint.Reset(connection);
        }

        public async Task DisposeAsync()
        {
        }
    }
}