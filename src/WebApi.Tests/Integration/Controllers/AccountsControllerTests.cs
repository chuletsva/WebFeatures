using Application.Features.Accounts.Register;
using Domian.Entities.Accounts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using WebApi.Tests.Common;
using Xunit;

namespace WebApi.Tests.Integration
{
    public class AccountsControllerTests : ControllerTestBase
    {
        public async Task Register_ShouldCreateNewUser_ReturnsUserIdAndToken()
        {
            // Arrange
            var client = CreateDefaultClient();

            var request = new RegisterCommand()
            {
                Email = "testUser@mail.com",
                Name = "test",
                Password = "12345"
            };

            // Act
            HttpResponseMessage response = await client.PostAsync("/accounts/register", request);

            var content = new { Id = default(Guid), Token = default(string) };

            await response.ReadAsJson(content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Id.Should().NotBeEmpty();
            content.Token.Should().NotBeEmpty();
        }
    }

    public static class Utilities
    {
        public static Task<HttpResponseMessage> PostAsync(this HttpClient client, string requestUri, object body)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(body), 
                Encoding.UTF8, 
                MediaTypeNames.Application.Json);

            return client.PostAsync(requestUri, content);
        }

        public static async Task<TResponse> ReadAsJson<TResponse>(this HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(content);
        }

        public static async Task<TResponse> ReadAsJson<TResponse>(this HttpResponseMessage response, TResponse anonymous)
        {
            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeAnonymousType(content, anonymous);
        }
    }
}