using Application.Interfaces.DataAccess;
using Application.Interfaces.Logging;
using Application.Interfaces.Security;
using Domian.Entities.Accounts;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace WebApi.Tests.Common
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => 
            {
                SetupInMemoryDbContext(services);
                SeedTestData(services);
                SetupMockAuthorization(services);
            });
        }

        private static void SetupInMemoryDbContext(IServiceCollection services)
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

            var hasher = scope.ServiceProvider.GetService<IPasswordHasher>();

            EntityData.Seed(context, hasher);
        }

        private static void SetupMockAuthorization(IServiceCollection services)
        {
            services.AddAuthentication(CustomAuthHandler.SchemaName)
                .AddScheme<AuthenticationSchemeOptions, CustomAuthHandler>(CustomAuthHandler.SchemaName, options => { });
        }
    }

    public static class EntityData
    {
        public static void Seed(DbContext context, IPasswordHasher hasher)
        {
            context.Add(new User()
            {
                Id = new Guid("7a23912f-e713-4a8f-81f6-17306964dc9a"),
                Name = "User",
                Email = "user@mail.com",
                PasswordHash = hasher.ComputeHash("12345")
            });

            context.SaveChanges();
        }
    }

    public class CustomAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SchemaName = "Test";
        public const string AuthorizationHeaderName = "Authorization";

        private readonly IDbContext _db;

        public CustomAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock, 
            IDbContext db) : base(options, logger, encoder, clock)
        {
            _db = db;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(AuthorizationHeaderName, out var headerString))
            {
                return AuthenticateResult.NoResult();
            }

            if (!AuthenticationHeaderValue.TryParse(headerString, out AuthenticationHeaderValue header))
            {
                return AuthenticateResult.NoResult();
            }

            if (!SchemaName.Equals(header.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.NoResult();
            }

            if (!Guid.TryParse(header.Parameter, out Guid userId))
            {
                return AuthenticateResult.Fail("Wrong id format");
            }

            User user = await _db.Users.FindAsync(userId);

            if (user == null)
            {
                return AuthenticateResult.Fail("User doesn't exist");
            }

            var claims = new[] 
            { 
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) 
            };
            var identity = new ClaimsIdentity(claims, SchemaName);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, SchemaName);

            return AuthenticateResult.Success(ticket);
        }
    }
}