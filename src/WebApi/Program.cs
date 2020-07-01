using Application.Interfaces.Security;
using Autofac.Extensions.DependencyInjection;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            await InitDatabase(host);

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task InitDatabase(IHost host)
        {
            using IServiceScope scope = host.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (!context.Database.IsInMemory())
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();

                IEnumerable<string> migrations = await context.Database.GetPendingMigrationsAsync();

                if (migrations.Any())
                {
                    await context.Database.MigrateAsync();
                }
            }

            var hasher = scope.ServiceProvider.GetService<IPasswordHasher>();

            await AppDbContextSeed.Seed(context, hasher);
        }
    }
}