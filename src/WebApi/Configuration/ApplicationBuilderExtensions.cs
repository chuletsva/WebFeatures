using Application.Exceptions;
using Application.Interfaces.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;

namespace WebApi.Configuration
{
    static class ApplicationBuilderExtensions
    {
        public static void UseExceptionHandling(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    switch (errorFeature.Error)
                    {
                        case ValidationException validation:

                            context.Response.StatusCode = StatusCodes.Status400BadRequest;

                            string body = JsonConvert.SerializeObject(validation.Error);

                            await context.Response.WriteAsync(body);

                            break;

                        case ODataExeption odata:

                            context.Response.StatusCode = StatusCodes.Status400BadRequest;

                            await context.Response.WriteAsync(odata.Message);

                            break;

                        case FailedAuthorizationException authorization:

                            context.Response.StatusCode = StatusCodes.Status400BadRequest;

                            await context.Response.WriteAsync(authorization.Message);

                            break;

                        case Exception ex:

                            var logger = context.RequestServices.GetService<ILogger<Startup>>();

                            logger.LogError(null, ex);

                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                            await context.Response.WriteAsync("Something went wrong");

                            break;
                    }
                });
            });
        }
    }
}