using Application.Behaviours;
using Application.Exceptions;
using Application.Interfaces.DataAccess;
using Application.Interfaces.Services;
using Application.Tests.Common.Stubs.Requests;
using AutoFixture;
using Domian.Entities.Accounts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Tests.Common.Base;
using Xunit;

namespace Application.Tests.Unit.Behaviours
{
    public class AuthorizationBehaviourTests : BehaviourTestBase
    {      
        [Fact]
        public async Task ShouldReturnNextDelegateResult_WhenUserExists()
        {
            // Arrange
            var usersSet = Fixture.Create<Mock<DbSet<User>>>();
            {
                var user = Fixture.Create<User>();
                
                usersSet.Setup(x => x.FindAsync(
                    It.IsAny<object[]>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(user);
            }
            
            var context = Fixture.Freeze<Mock<IDbContext>>();
            {
                context.Setup(x => x.Users).Returns(usersSet.Object);
            }

            var currentUser = Fixture.Freeze<Mock<ICurrentUser>>();
            {
                currentUser.Setup(x => x.IsAuthenticated).Returns(true);
            }

            var sut = Fixture.Create<AuthorizationBehaviour<AuthorizationRequest, int>>();

            int expected = Fixture.Create<int>();

            // Act
            int actual = await sut.Handle(new AuthorizationRequest(), new CancellationToken(), () => Task.FromResult(expected));

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldThrow_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var sut = Fixture.Create<AuthorizationBehaviour<AuthorizationRequest, int>>();

            // Act
            Func<Task<int>> act = () => sut.Handle(new AuthorizationRequest(), new CancellationToken(), () => Task.FromResult(0));

            // Assert
            act.Should().Throw<FailedAuthorizationException>().WithMessage("User is not authorized");
        }

        [Fact]
        public void ShouldThrow_WhenUserDoesntExist()
        {
            // Arrange
            var usersSet = Fixture.Create<Mock<DbSet<User>>>();
            {
                usersSet.Setup(x => x.FindAsync(
                    It.IsAny<object[]>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(() => null);
            }

            var context = Fixture.Freeze<Mock<IDbContext>>();
            {
                context.Setup(x => x.Users).Returns(usersSet.Object);   
            }

            var currentUser = Fixture.Freeze<Mock<ICurrentUser>>();
            {
                currentUser.Setup(x => x.IsAuthenticated).Returns(true);
            }

            var sut = Fixture.Create<AuthorizationBehaviour<AuthorizationRequest, int>>();

            // Act
            Func<Task<int>> act = () => sut.Handle(new AuthorizationRequest(), new CancellationToken(), () => Task.FromResult(0));

            // Assert
            act.Should().Throw<FailedAuthorizationException>().WithMessage("User is not authorized");
        }
    }
}