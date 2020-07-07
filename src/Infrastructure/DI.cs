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
using System;

namespace Infrastructure
{
    public static class DI
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddCommonServices(services);
            AddDataAccess(services, configuration);
            AddSecurity(services);
            AddLogging(services);
        }

        private static void AddCommonServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUserService>();

            services.AddSingleton<IDateTime, DateTimeService>();
        }

        private static void AddDataAccess(IServiceCollection services, IConfiguration configuration)
        {
            string env = configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");

            string connectionString = env switch
            {
                "Development" => configuration.GetConnectionString("Default"),
                "Testing" => configuration.GetConnectionString("Testing"),
                _ => throw new IndexOutOfRangeException()
            };

            services.AddDbContext<AppDbContext>(builder =>
            {
                builder.UseNpgsql(
                    connectionString,
                    opt => opt.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
            });

            services.AddScoped<IDbContext>(provider => provider.GetService<AppDbContext>());
        }

        private static void AddSecurity(IServiceCollection services)
        {
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
        }

        private static void AddLogging(IServiceCollection services)
        {
            services.AddSingleton(typeof(ILogger<>), typeof(LoggerAdapter<>));
        }
    }
}