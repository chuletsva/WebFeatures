using Infrastructure.Logging;
using Infrastructure.Tests.Common.Attributes;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Infrastructure.Tests.Unit.Logging
{
    public class LoggerAdapterTests
    {
        [Theory, AutoMoq]
        public void ShouldCreateLogger(Mock<ILoggerFactory> loggerFactory)
        {
            // Act
            new LoggerAdapter<LoggerAdapterTests>(loggerFactory.Object);

            // Assert
            loggerFactory.Verify(x => x.CreateLogger(typeof(LoggerAdapterTests).FullName), Times.Once);
        }
    }
}
