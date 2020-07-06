using Application.Interfaces.DataAccess;
using Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Tests.Common
{
    public class IntegrationTestBase
    {
        private static IServiceProvider _serviceProvider;

        static IntegrationTestBase()
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
        }

        protected static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();

            return await mediator.Send(request);
        }

        protected static async Task<TEntity> FindAsync<TEntity>(Guid id) where TEntity : class
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            var db = scope.ServiceProvider.GetService<IDbContext>();

            return await db.FindAsync<TEntity>(id);
        }
    }
}