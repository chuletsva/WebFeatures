using Application.Interfaces.DataAccess;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DI
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDataAccess(services, configuration);
        }

        private static void AddDataAccess(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AppConnectionString");

            services.AddDbContext<BaseDbContext>(builder =>
            {
                builder.UseNpgsql(connectionString);
            });

            services.AddScoped<IDbContext, BaseDbContext>();
        }
    }
}