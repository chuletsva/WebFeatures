using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Tests.Common
{
    public class ControllerTestBase
    {
        protected readonly CustomWebApplicationFactory<Startup> Factory;

        protected ControllerTestBase()
        {
            Factory = new CustomWebApplicationFactory<Startup>();
        }

        protected HttpClient CreateDefaultClient()
        {
            var client = Factory.CreateClient(
                new WebApplicationFactoryClientOptions()
                {
                    BaseAddress = new Uri("https://localhost/5001")
                });

            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue(CustomAuthHandler.SchemaName);

            return client;
        }
    }
}