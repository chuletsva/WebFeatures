using Application.Behaviours;
using Application.Exceptions;
using Application.Interfaces.DataAccess;
using Application.Interfaces.Services;
using Application.Tests.Common.Stubs.Requests;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domian.Entities.Accounts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.Unit.Behaviours
{
    public class AuthorizationBehaviourTests
    {
        [Fact]
        public async Task ShouldCallNextDelegate_WhenUserExists()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var usersSet = fixture.Create<Mock<DbSet<User>>>();

            var user = fixture.Create<User>();

            usersSet.Setup(x => x.FindAsync(
                It.IsAny<object[]>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var context = fixture.Freeze<Mock<IDbContext>>();

            context.Setup(x => x.Users).Returns(usersSet.Object);

            var currentUser = fixture.Freeze<Mock<ICurrentUser>>();

            currentUser.Setup(x => x.IsAuthenticated).Returns(true);

            var sut = fixture.Create<AuthorizationBehaviour<AuthorizedRequest, bool>>();

            // Act
            bool isNextCalled = await sut.Handle(
                new AuthorizedRequest(),
                new CancellationToken(),
                async () => true);

            // Assert
            isNextCalled.Should().BeTrue();
        }

        [Fact]
        public void ShouldThrow_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var sut = fixture.Create<AuthorizationBehaviour<AuthorizedRequest, bool>>();

            // Act
            Func<Task<bool>> actual = () => sut.Handle(
                new AuthorizedRequest(),
                new CancellationToken(),
                async () => true);

            // Assert
            actual.Should().Throw<FailedAuthorizationException>().WithMessage("User is not authorized");
        }

        [Fact]
        public void ShouldThrow_WhenUserDoesntExist()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var usersSet = fixture.Create<Mock<DbSet<User>>>();

            usersSet.Setup(x => x.FindAsync(
                It.IsAny<object[]>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

            var context = fixture.Freeze<Mock<IDbContext>>();

            context.Setup(x => x.Users).Returns(usersSet.Object);

            var currentUser = fixture.Freeze<Mock<ICurrentUser>>();

            currentUser.Setup(x => x.IsAuthenticated).Returns(true);

            var sut = fixture.Create<AuthorizationBehaviour<AuthorizedRequest, bool>>();

            // Act
            Func<Task<bool>> actual = () => sut.Handle(
                new AuthorizedRequest(),
                new CancellationToken(),
                async () => true);

            // Assert
            actual.Should().Throw<FailedAuthorizationException>().WithMessage("User is not authorized");
        }
    }
}