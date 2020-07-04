using Application.Interfaces.Logging;
using LinqToQuerystring;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using WebApi.Exceptions;

namespace WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    class ODataAttribute : Attribute, IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            var objectResult = context.Result as ObjectResult;

            if (objectResult == null)
            {
                throw new InvalidOperationException("Response type should be 'ObjectResult'");
            }

            if (objectResult.Value == null)
            {
                return;
            }

            Type responseType = objectResult.Value.GetType();

            if (!responseType.GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IQueryable<>)))
            {
                throw new InvalidOperationException("Response value type should implement IQueryable<>");
            }

            Type elementType = responseType.GetGenericArguments()[0];

            var queryString = context.HttpContext.Request.QueryString;

            if (!queryString.HasValue)
            {
                return;
            }

            string query = Uri.UnescapeDataString(queryString.Value.TrimStart('?'));

            try
            {
                objectResult.Value = ((IQueryable)objectResult.Value).LinqToQuerystring(elementType, query);
            }
            catch (Exception ex)
            {
                var logger = context.HttpContext.RequestServices.GetService<ILogger<ODataAttribute>>();

                logger.LogError("Error while applying odata", ex);

                throw new ODataExeption($"Invalid odata request '{query}'");
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}