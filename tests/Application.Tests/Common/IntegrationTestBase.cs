using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

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
    }
}