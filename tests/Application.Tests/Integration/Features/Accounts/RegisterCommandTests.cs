using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Models.Dto;
using Application.Features.Accounts.Register;
using Application.Tests.Common.Base;
using Domian.Entities.Accounts;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Integration.Features.Accounts
{
    public class RegisterCommandTests : RequestTestBase
    {
        [Fact]
        public async Task ShouldCreateUser_WhenEmailDoesntExist()
        {
            // Arrange
            var request = new RegisterCommand()
            {
                Name = "user",
                Email = "user@email",
                Password = "12345"
            };

            // Act
            UserInfoDto userDto = await SendAsync(request);

            User user = await FindAsync<User>(x => x.Email == request.Email);

            // Assert
            user.Should().NotBeNull();
            user.Id.Should().Be(userDto.Id);
            user.Name.Should().Be(request.Name);
            user.Email.Should().Be(request.Email);
            user.PasswordHash.Should().NotBeEmpty();
        }

        [Fact]
        public void ShouldThrow_WhenInvalidCredentials()
        {
            FluentActions.Awaiting(() => SendAsync(new RegisterCommand()))
                .Should().Throw<ValidationException>()
                .And.Error.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldThrow_WhenEmailAlreadyExists()
        {
            // Arrange
            var request = new RegisterCommand()
            {
                Name = "Name",
                Email = "default@user",
                Password = "12345"
            };

            await LoginAsync(request.Email);

            // Act
            Func<Task<UserInfoDto>> act = () => SendAsync(request);

            // Assert
            act.Should().Throw<ValidationException>().And.Error.Message.Should().Be("Email already exists");
        }
    }
}