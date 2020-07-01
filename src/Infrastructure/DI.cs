using Application.Interfaces.DataAccess;
using Application.Interfaces.Logging;
using Application.Interfaces.Security;
using Application.Interfaces.Services;
using Infrastructure.DataAccess;
using Infrastructure.Logging;
using Infrastructure.Security;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DI
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddCommonServices(services);
            AddInMemoryContext(services);
            //AddPostgreContext(services, configuration);
            AddSecurity(services);
            AddLogging(services);
        }

        private static void AddCommonServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUserService>();

            services.AddSingleton<IDateTime, DateTimeService>();
        }

        private static void AddInMemoryContext(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(builder =>
            {
                builder.UseInMemoryDatabase("webfeatures_db");
            }, ServiceLifetime.Scoped);

            services.AddScoped<IDbContext>(provider => provider.GetService<AppDbContext>());
        }

        private static void AddPostgreContext(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AppConnectionString");

            services.AddDbContext<AppDbContext>(builder =>
            {
                builder.UseNpgsql(connectionString);
            });

            services.AddScoped<IDbContext>(provider => provider.GetService<AppDbContext>());
        }

        private static void AddSecurity(IServiceCollection services)
        {
            services.AddDataProtection();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
        }

        private static void AddLogging(IServiceCollection services)
        {
            services.AddSingleton(typeof(ILogger<>), typeof(LoggerAdapter<>));
        }
    }
}