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
    public class Register : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public Register(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ShouldCreateNewUser_ReturnsUserIdAndToken()
        {
            // Arrange
            var client = _factory.GetDefaultClient();

            var request = new RegisterCommand()
            {
                Email = "user@email",
                Name = "user",
                Password = "12345"
            };

            // Act
            HttpResponseMessage response = await client.PostAsync("/api/accounts/register", request);

            var content = await response.ReadContentAsAnonymous(new
            {
                id = default(Guid),
                access_token = default(string)
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.id.Should().NotBeEmpty();
            content.access_token.Should().NotBeEmpty();
        }
    }
}