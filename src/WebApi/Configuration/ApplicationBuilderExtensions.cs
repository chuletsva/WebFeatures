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

                    var logger = context.RequestServices.GetService<ILogger<Startup>>();

                    logger.LogError("Request error", errorFeature.Error);

                    (int Code, string Body) response = errorFeature.Error switch
                    {
                        ValidationException validation => (400, JsonConvert.SerializeObject(validation.Error)),
                        ODataExeption odata => (400, odata.Message),
                        FailedAuthorizationException authorization => (400, authorization.Message),
                        Exception other => (500, "Something went wrong")
                    };

                    context.Response.StatusCode = response.Code;

                    await context.Response.WriteAsync(response.Body);
                });
            });
        }
    }
}