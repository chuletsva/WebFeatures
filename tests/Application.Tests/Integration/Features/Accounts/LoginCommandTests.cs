using System;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Features.Accounts.Login;
using Application.Features.Accounts.Register;
using Application.Tests.Common;
using Domian.Entities.Accounts;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Integration.Features.Accounts
{
    public class LoginCommandTests : RequestTestBase
    {
        [Fact]
        public async Task ShouldLoginUser_WhenUserExists()
        {
            // Arrange & act
            var register = new RegisterCommand
            {
                Name = "User",
                Email = "user@email.com",
                Password = "12345"
            };

            await SendAsync(register);

            var login = new LoginCommand
            {
                Email = register.Email,
                Password = register.Password
            };

            var dto = await SendAsync(login);

            var user = await FindAsync<User>(x => x.Email == register.Email);

            // Assert
            user.Should().NotBeNull();
            user.Id.Should().Be(dto.Id);
        }

        [Fact]
        public void ShouldThrow_WhenEmailDoesntExist()
        {
            // Arrange
            var login = new LoginCommand
            {
                Email = "wrong@mail.com",
                Password = "12345"
            };

            // Act
            Func<Task<UserLoginDto>> actual = () => SendAsync(login);

            // Assert
            actual.Should()
                .Throw<ValidationException>()
                .Where(x => x.Error != null)
                .And.Error.Message.Should().Be("Wrong login or password");
        }

        [Fact]
        public async Task ShouldThrow_WhenWrongPassword()
        {
            // Arrange & act
            var register = new RegisterCommand
            {
                Name = "User",
                Email = "user@email.com",
                Password = "12345"
            };

            await SendAsync(register);

            var login = new LoginCommand
            {
                Email = "user@email.com",
                Password = "1234"
            };

            Func<Task<UserLoginDto>> actual = () => SendAsync(login);

            // Assert
            actual.Should()
                .Throw<ValidationException>()
                .Where(x => x.Error != null)
                .And.Error.Message.Should().Be("Wrong login or password");
        }
    }
}