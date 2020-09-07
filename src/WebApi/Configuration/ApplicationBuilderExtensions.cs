using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Logging;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.OData;
using WebApi.Pagination;

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
                            var options = context.RequestServices.GetService<IOptions<JsonOptions>>();

                            response.code = StatusCodes.Status400BadRequest;
                            response.body = JsonSerializer.Serialize(validation.Error, options.Value.JsonSerializerOptions);
                            break;

                        case FailedAuthorizationException authorization:
                            response.code = StatusCodes.Status400BadRequest;
                            response.body = authorization.Message;
                            break;

                        case ODataException odata:
                            response.code = StatusCodes.Status400BadRequest;
                            response.body = odata.Message;
                            break;

                        case PaginationException pagination:
                            response.code = StatusCodes.Status400BadRequest;
                            response.body = pagination.Message;
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
    }
}