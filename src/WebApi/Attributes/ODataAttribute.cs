using Application.Interfaces.Logging;
using LinqToQuerystring;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using WebApi.Exceptions;

namespace WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal class ODataAttribute : Attribute, IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (!(context.Result is ObjectResult objectResult))
            {
                throw new InvalidOperationException("Response type should be 'ObjectResult'");
            }

            if (objectResult.Value == null) return;

            Type responseType = objectResult.Value.GetType();

            if (!responseType.GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IQueryable<>)))
            {
                throw new InvalidOperationException("Response value type should implement IQueryable<>");
            }

            QueryString queryString = context.HttpContext.Request.QueryString;

            if (!queryString.HasValue) return;

            string query = Uri.UnescapeDataString(queryString.Value);

            try
            {
                Type elementType = responseType.GetGenericArguments()[0];

                objectResult.Value = ((IQueryable)objectResult.Value).LinqToQuerystring(elementType, query);
            }
            catch (Exception ex)
            {
                var logger = context.HttpContext.RequestServices.GetService<ILogger<ODataAttribute>>();

                logger.LogError("Error while applying odata", ex);

                throw new ODataException($"Invalid odata request '{query}'");
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}