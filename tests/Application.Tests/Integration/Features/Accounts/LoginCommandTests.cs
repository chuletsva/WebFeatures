using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Models.Dto;
using Application.Features.Accounts.Login;
using Application.Tests.Common.Base;
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
            // Arrange
            var request = new LoginCommand { Email = "default@user", Password = "12345" };

            UserInfoDto userDto = await SendAsync(request);

            User user = await FindAsync<User>(x => x.Email == request.Email);

            // Assert
            user.Should().NotBeNull();
            user.Id.Should().Be(userDto.Id);
        }

        [Fact]
        public void ShouldThrow_WhenEmailDoesntExist()
        {
            // Arrange
            var request = new LoginCommand { Email = "wrong@email", Password = "12345" };

            // Act
            Func<Task<UserInfoDto>> act = () => SendAsync(request);

            // Assert
            act.Should().Throw<ValidationException>().And.Error.Message.Should().Be("Wrong login or password");
        }

        [Fact]
        public void ShouldThrow_WhenWrongPassword()
        {
            // Arrange
            var request = new LoginCommand { Email = "default@user", Password = "1234" };

            // Act
            Func<Task<UserInfoDto>> act = () => SendAsync(request);

            // Assert
            act.Should().Throw<ValidationException>().And.Error.Message.Should().Be("Wrong login or password");
        }
    }
}