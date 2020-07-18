using Application.Interfaces.Security;
using Autofac.Extensions.DependencyInjection;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = CreateLogger();

            try
            {
                Log.Information("Starting up");

                IHost host = CreateHostBuilder(args).Build();

                await InitDatabase(host);

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static ILogger CreateLogger()
        {
            var configuration = new LoggerConfiguration();

            configuration.WriteTo.Console(LogEventLevel.Information);

            string logsPath = CreateLogFilePath();

            configuration.WriteTo.File(
                path: logsPath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                restrictedToMinimumLevel: LogEventLevel.Information);

            ILogger logger = configuration.CreateLogger();

            logger.Information($"Writing logs to '{logsPath}'");

            return logger;
        }

        private static string CreateLogFilePath()
        {
            string projectDir = Path.GetDirectoryName(typeof(Program).Assembly.Location);

            string logsDir = Path.Combine(projectDir, "Logs");

            if (!Directory.Exists(logsDir)) Directory.CreateDirectory(logsDir);

            return Path.Combine(logsDir, "log.txt");
        }

        private static async Task InitDatabase(IHost host)
        {
            using IServiceScope scope = host.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.EnsureDeletedAsync();

            await context.Database.MigrateAsync();

            var hasher = scope.ServiceProvider.GetService<IPasswordHasher>();

            await AppDbContextSeed.Seed(context, hasher);
        }
    }
}