using Infrastructure.DataAccess;
using Infrastructure.Logging;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Application.Common.Interfaces.BackgroundJobs;
using Application.Common.Interfaces.CommonServices;
using Application.Common.Interfaces.DataAccess;
using Application.Common.Interfaces.Logging;
using Application.Common.Interfaces.Security;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.BackgroundJobs;
using Infrastructure.CommonServices;

namespace Infrastructure
{
    public static class DI
    {
        public static void RegisterBackgroundJobs(
            this IServiceCollection services,
            IConfiguration configuration,
            bool prepareSchema = false)
        {
            services.AddScoped<IBackgroundJobManager, BackgroundJobManager>();

            var connectionString = configuration.GetConnectionString("Hangfire");

            services.AddHangfire(conf =>
            {
                conf.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                   .UseSimpleAssemblyNameTypeSerializer()
                   .UseRecommendedSerializerSettings()
                   .UsePostgreSqlStorage(connectionString, new PostgreSqlStorageOptions()
                   {
                       QueuePollInterval = TimeSpan.FromMilliseconds(1),
                       PrepareSchemaIfNecessary = prepareSchema
                   });
            });
        }

        public static void RegisterCommonServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUserService>();

            services.AddSingleton<IDateTime, DateTimeService>();
        }

        public static void RegisterDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Application");

            services.AddDbContext<AppDbContext>(builder =>
            {
                builder.UseNpgsql(
                    connectionString,
                    opt => opt.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
            });

            services.AddScoped<IDbContext>(provider => provider.GetService<AppDbContext>());
        }

        public static void RegisterLogging(this IServiceCollection services)
        {
            services.AddLogging();
            services.AddSingleton(typeof(ILogger<>), typeof(LoggerAdapter<>));
        }

        public static void RegisterSecurity(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
        }
    }
}