using Application.Behaviours;
using Application.Interfaces.BackgroundJobs;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Application
{
    public static class DI
    {
        public static void RegisterApplication(this IServiceCollection services)
        {
            RegisterPipeline(services);
            RegisterMapperProfiles(services);
            RegisterValidators(services);
            RegisterBackgroundJobs(services);
        }

        private static void RegisterPipeline(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Common
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));

            // Commands
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(SaveChangesBehaviour<,>));
        }

        private static void RegisterMapperProfiles(IServiceCollection services)
        {
            var configuration = new MapperConfiguration(config =>
            {
                Type[] profileTypes = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(x => x.IsSubclassOf(typeof(Profile)))
                    .ToArray();

                foreach (Type profileType in profileTypes)
                {
                    config.AddProfile(profileType);
                }
            });

            configuration.AssertConfigurationIsValid();

            services.AddSingleton(configuration.CreateMapper());
        }

        private static void RegisterValidators(IServiceCollection services)
        {
            services.Scan(scan =>
            {
                scan.FromAssembliesOf(typeof(DI))
                   .AddClasses(x => x.AssignableTo(typeof(IValidator<>)), publicOnly: false)
                   .AsImplementedInterfaces()
                   .WithScopedLifetime();
            });
        }

        private static void RegisterBackgroundJobs(IServiceCollection services)
        {
            services.Scan(scan =>
            {
                scan.FromAssembliesOf(typeof(DI))
                    .AddClasses(x => x.AssignableTo(typeof(IBackgroundJob<>)), publicOnly: false)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });
        }
    }
}