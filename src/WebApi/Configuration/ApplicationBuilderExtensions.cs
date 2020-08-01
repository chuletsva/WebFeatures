using Application.Exceptions;
using Application.Features.System.StartRecurringJobs;
using Application.Interfaces.Logging;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using WebApi.Exceptions;

namespace WebApi.Configuration
{
    internal static class ApplicationBuilderExtensions
    {
        public static void UseExceptionHandling(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    (int code, string body) response = (StatusCodes.Status500InternalServerError, null);

                    switch (errorFeature.Error)
                    {
                        case ValidationException validation:
                            var settings = context.RequestServices.GetService<JsonSerializerSettings>();

                            response.code = StatusCodes.Status400BadRequest;
                            response.body = JsonConvert.SerializeObject(validation.Error, settings);
                            break;

                        case FailedAuthorizationException authorization:
                            response.code = StatusCodes.Status400BadRequest;
                            response.body = authorization.Message;
                            break;

                        case ODataException odata:
                            response.code = StatusCodes.Status400BadRequest;
                            response.body = odata.Message;
                            break;

                        case Exception ex:
                            var logger = context.RequestServices.GetService<ILogger<Startup>>();
                            logger.LogError("Unhandled error", errorFeature.Error);
                            break;
                    }

                    context.Response.StatusCode = response.code;

                    if (response.body != null)
                    {
                        await context.Response.WriteAsync(response.body);
                    }
                });
            });
        }

        public static void StartRecurringJobs(this IApplicationBuilder app)
        {
            var mediator = app.ApplicationServices.GetService<IMediator>();

            mediator.Send(new StartRecurringJobsCommand()).Wait();
        }
    }
}