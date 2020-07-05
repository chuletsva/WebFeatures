using FluentAssertions;
using Infrastructure.Services;
using Infrastructure.Tests.Common.Attributes;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Security.Claims;
using Xunit;

namespace Infrastructure.Tests.Unit.Services
{
    public class CurrentUserServiceTests
    {
        [Theory, AutoMoq]
        public void ShouldSetCurrentUser_WhenUserClaimExists(
            Guid userId,
            Mock<HttpContext> context,
            Mock<IHttpContextAccessor> contextAccessor)
        {
            // Arrange
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) });

            var principal = new ClaimsPrincipal(identity);

            context.SetupGet(x => x.User).Returns(principal);

            contextAccessor.SetupGet(x => x.HttpContext).Returns(context.Object);

            // Act
            var sut = new CurrentUserService(contextAccessor.Object);

            // Assert
            sut.UserId.Should().Be(userId);
            sut.IsAuthenticated.Should().BeTrue();
        }
    }
}