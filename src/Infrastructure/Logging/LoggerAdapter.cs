using Microsoft.Extensions.Logging;
using System;

namespace Infrastructure.Logging
{
    public class LoggerAdapter<T> : Application.Common.Interfaces.Logging.ILogger<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerAdapter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<T>();
        }

        public void LogError(string message, Exception exception, params object[] arguments)
        {
            _logger.LogError(exception, message, arguments);
        }

        public void LogInformation(string message, params object[] arguments)
        {
            _logger.LogInformation(message, arguments);
        }

        public void LogWarning(string message, params object[] arguments)
        {
            _logger.LogWarning(message, arguments);
        }
    }
}