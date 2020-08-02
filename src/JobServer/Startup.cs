using System;
using Application;
using Hangfire;
using Infrastructure;
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
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireServer(new BackgroundJobServerOptions()
            {
                ServerName = $"hangfire_{DateTime.Now:dd.MM.yyyy}",
                SchedulePollingInterval = TimeSpan.FromSeconds(5),
                WorkerCount = 5
            });

            app.UseHangfireDashboard("/jobs", new DashboardOptions()
            {
                IsReadOnlyFunc = ctx => true,
                AppPath = null
            });
        }
    }
}