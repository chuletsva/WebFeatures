using Application.Common.Models.Results;
using Application.Features.Accounts.Register;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Tests.Common;
using Xunit;

namespace WebApi.Tests.Integration.Controllers.Accounts
{
    public class Register : ControllerTestBase
    {
        [Fact]
        public async Task ShouldCreateNewUser_ReturnsToken()
        {
            // Arrange
            var client = Factory.GetDefaultClient();

            var request = new RegisterCommand()
            {
                Email = "user@email",
                Name = "user",
                Password = "12345"
            };

            // Act
            HttpResponseMessage response = await client.PostAsync("/api/accounts/register", request);

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

        [Fact]
        public async Task ShouldReturnError_WhenEmailAlreadyExists()
        {
            // Arrange
            var client = Factory.GetDefaultClient();

            var request = new RegisterCommand()
            {
                Email = "default@user",
                Name = "user",
                Password = "12345"
            };

            // Act
            HttpResponseMessage response = await client.PostAsync("/api/accounts/register", request);

            ValidationError error = await response.ReadContent<ValidationError>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            error.Should().NotBeNull();
            error.Message.Should().Be("Email already exists");
        }
    }
}