using Application.Exceptions;
using Application.Features.Accounts.Login;
using Application.Tests.Common;
using Domian.Entities.Accounts;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.Integration.Features.Accounts
{
    public class RegisterCommandTests : RequestTestBase
    {
        [Fact]
        public async Task ShouldCreateUser_WhenUserWithProvidedEmailDoesntExists()
        {
            // Arrange
            var request = new LoginCommand()
            {
                Email = "user@mail.com",
                Password = "12345"
            };

            // Act
            UserLoginDto dto = await SendAsync(request);

            User user = await FindAsync<User>(x => x.Email == request.Email);

            // Assert
            dto.Should().NotBeNull();
            dto.Roles.Should().NotBeEmpty();

            user.Should().NotBeNull();
            user.Id.Should().Be(dto.Id);
            user.Email.Should().Be(request.Email);
        }

        [Fact]
        public void ShouldThrow_WhenInvalidCredentials()
        {
            // Act
            Func<Task<UserLoginDto>> actual = () => SendAsync(new LoginCommand());

            // Assert
            actual.Should().Throw<ValidationException>().And.Error.Should().NotBeNull();
        }
    }
}