using Application.Exceptions;
using Application.Features.Accounts.Register;
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
            var request = new RegisterCommand()
            {
                Name = "user",
                Email = "user@mail.com",
                Password = "12345"
            };

            // Act
            UserCreateDto dto = await SendAsync(request);

            User user = await FindAsync<User>(x => x.Email == request.Email);

            // Assert
            user.Should().NotBeNull();
            user.Id.Should().Be(dto.Id);
            user.Name.Should().Be(request.Name);
            user.Email.Should().Be(request.Email);
            user.PasswordHash.Should().NotBeEmpty();
        }

        [Fact]
        public void ShouldThrow_WhenInvalidCredentials()
        {
            // Arrange
            var register = new RegisterCommand();

            // Act
            Func<Task<UserCreateDto>> actual = () => SendAsync(register);

            // Assert
            actual.Should().Throw<ValidationException>().And.Error.Should().NotBeNull();
        }
    }
}