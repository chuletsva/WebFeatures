using Application.Common.Models.Results;
using Application.Features.Accounts.Login;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Tests.Common;
using Xunit;

namespace WebApi.Tests.Integration.Controllers.Accounts
{
    public class Login : ControllerTestBase
    {
        [Fact]
        public async Task ShouldReturnAccessToken_WhenUserIsRegistered()
        {
            // Arrange
            HttpClient client = Factory.GetDefaultClient();

            var request = new LoginCommand() { Email = "default@user", Password = "12345" };

            // Act
            HttpResponseMessage response = await client.PostAsync("api/accounts/login", request);

            var content = await response.ReadContent(new
            {
                id = default(Guid),
                access_token = default(string)
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.id.Should().NotBeEmpty();
            content.access_token.Should().NotBeEmpty();

        }

        [Theory]
        [InlineData("default@user", "wrong_password")]
        [InlineData("wrong@email", "12345")]
        public async Task ShouldReturnError_WhenInvalidCredentials(string email, string password)
        {
            // Arrange
            var client = Factory.GetDefaultClient();

            var request = new LoginCommand() { Email = email, Password = password };

            // Act
            HttpResponseMessage response = await client.PostAsync("api/accounts/login", request);

            ValidationError error = await response.ReadContent<ValidationError>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            error.Should().NotBeNull();
            error.Message.Should().Be("Wrong login or password");
        }
    }
}