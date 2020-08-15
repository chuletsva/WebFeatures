using Domian.Entities.Accounts;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Common.Interfaces.Security;
using WebApi.Authentication;
using Moq;

namespace WebApi.Tests.Common
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                SetupPasswordHasher(services);
                SetupDbContext(services);
                SeedTestData(services);
            });
        }

        private static void SetupPasswordHasher(IServiceCollection services)
        {
            var hasherDescriptor = services.Single(x => x.ServiceType == typeof(IPasswordHasher));

            services.Remove(hasherDescriptor);

            var hasher = new Mock<IPasswordHasher>();
            {
                hasher.Setup(x => x.ComputeHash(It.IsAny<string>()))
                    .Returns<string>(x => x);

                hasher.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns((string hash, string expected) => hash == expected);
            }

            services.AddSingleton(hasher.Object);
        }

        private static void SetupDbContext(IServiceCollection services)
        {
            var optionsDescriptor = services.Single(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>));

            services.Remove(optionsDescriptor);

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            services.AddSingleton(options);
        }

        private static void SeedTestData(IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();

            var context = scope.ServiceProvider.GetService<AppDbContext>();

            EntityTestData.Seed(context);
        }

        public HttpClient GetDefaultClient()
        {
            return CreateClient();
        }

        public async Task<HttpClient> GetAuthenticatedClient()
        {
            return await GetAuthenticatedClient("default@user");
        }

        public async Task<HttpClient> GetAuthenticatedClient(string email)
        {
            HttpClient client = CreateDefaultClient();

            string token = await GetAccessToken(email);

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            return client;
        }

        private async Task<string> GetAccessToken(string email)
        {
            using IServiceScope scope = Services.CreateScope();

            var context = scope.ServiceProvider.GetService<AppDbContext>();

            User user = await context.Users
                .Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .SingleOrDefaultAsync(x => x.Email == email) ??
                throw new Exception("User doesn't exist");

            var claims = new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };

            claims.AddRange(user.UserRoles.Select(x => new Claim(ClaimTypes.Role, x.Role.Name)));

            var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

            var tokenProvider = scope.ServiceProvider.GetService<ITokenProvider>();

            return tokenProvider.CreateToken(identity);
        }
    }
}