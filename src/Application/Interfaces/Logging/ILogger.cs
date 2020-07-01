using System;

namespace Application.Interfaces.Logging
{
    public interface ILogger<T>
    {
        void LogInformation(string message, params object[] arguments);
        void LogWarning(string message, params object[] arguments);
        void LogError(string message, Exception exception, params object[] arguments);
    }
}
