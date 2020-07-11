using Application.Behaviours;
using Application.Interfaces.Logging;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using MediatR;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.Unit.Behaviours
{
    public class LoggingBehaviourTests
    {
        [Fact]
        public async Task ShouldReturnNextDelegateResult()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var sut = fixture.Create<LoggingBehaviour<int, int>>();

            int expected = fixture.Create<int>();

            // Act
            int actual = await sut.Handle(0, new CancellationToken(), () => Task.FromResult(expected));

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task ShouldCallLoggerBeforeAndAfterNextDelegate()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var messages = new List<string>();

            var logger = fixture.Freeze<Mock<ILogger<int>>>();

            logger.Setup(x => x.LogInformation(
                It.IsAny<string>(),
                It.IsAny<object[]>())).Callback(() => messages.Add("logger"));

            var sut = fixture.Create<LoggingBehaviour<int, int>>();

            RequestHandlerDelegate<int> next = () =>
            {
                messages.Add("next");

                return Task.FromResult(0);
            };

            // Act
            await sut.Handle(0, new CancellationToken(), next);

            // Assert
            messages.Should().Equal(new[] { "logger", "next", "logger" });
        }
    }
}