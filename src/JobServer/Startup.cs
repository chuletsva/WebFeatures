using System;
using System.Security.Claims;
using Application;
using Hangfire;
using Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JobServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterApplication();

            services.RegisterBackgroundJobs(Configuration);
            services.RegisterCommonServices();
            services.RegisterDataAccess(Configuration);
            services.RegisterLogging();
            services.RegisterSecurity();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Map("/auth", authApp =>
            {
                authApp.Run(async context =>
                {
                    var identity = new ClaimsIdentity(
                        new[] { new Claim(ClaimTypes.Name, "guest") },
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    await context.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity));

                    context.Response.Redirect("/jobs");
                });
            });

            app.UseAuthentication();

            app.UseHangfireServer(new BackgroundJobServerOptions()
            {
                ServerName = $"hangfire_{DateTime.Now:dd.MM.yyyy}",
                SchedulePollingInterval = TimeSpan.FromSeconds(5),
                WorkerCount = 5
            });

            app.UseHangfireDashboard("/jobs", new DashboardOptions()
            {
                //Authorization = new[] { new AuthorizationFilter() },
                IsReadOnlyFunc = ctx => true,
                AppPath = null
            });
        }
    }
}