using AutoFixture;
using FluentAssertions;
using MediatR;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Behaviours;
using Application.Common.Interfaces.Logging;
using Application.Tests.Common.Base;
using Xunit;

namespace Application.Tests.Unit.Behaviours
{
    public class LoggingBehaviourTests : BehaviourTestBase
    {
        [Fact]
        public async Task ShouldReturnNextDelegateResult()
        {
            // Arrange
            var sut = Fixture.Create<LoggingBehaviour<int, int>>();

            int expected = Fixture.Create<int>();

            // Act
            int actual = await sut.Handle(0, new CancellationToken(), () => Task.FromResult(expected));

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task ShouldCallLoggerBeforeAndAfterNextDelegate()
        {
            // Arrange
            var messages = new List<string>();

            var logger = Fixture.Freeze<Mock<ILogger<int>>>();

            logger.Setup(x => x.LogInformation(
                It.IsAny<string>(),
                It.IsAny<object[]>())).Callback(() => messages.Add("logger"));

            var sut = Fixture.Create<LoggingBehaviour<int, int>>();

            RequestHandlerDelegate<int> next = () =>
            {
                messages.Add("next");

                return Task.FromResult(0);
            };

            // Act
            await sut.Handle(0, new CancellationToken(), next);

            // Assert
            messages.Should().Equal("logger", "next", "logger");
        }
    }
}