using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Authentication;
using WebApi.Scheduling;
using WebApi.Scheduling.Tasks;
using WebApi.Settings;

namespace WebApi.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterScheduledTasks(this IServiceCollection services)
        {
            services.AddHostedService<ScheduledTasksService>();
            services.AddScoped<Tick>();
        }

        public static void RegisterJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITokenProvider, TokenProvider>();

            var jwtSettingsSection = configuration.GetSection("JwtSettings");

            services.Configure<JwtSettings>(jwtSettingsSection);

            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    };
                });
        }
    }
}
