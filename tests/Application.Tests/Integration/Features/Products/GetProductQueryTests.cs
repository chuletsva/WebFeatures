using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Features.Products.GetProduct;
using Application.Tests.Common.Base;
using Application.Tests.Common.Helpers;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Integration.Features.Products
{
    using static EntityTestData;

    public class GetProductQueryTests : RequestTestBase
    {
        [Fact]
        public async Task ShouldReturnProduct()
        {
            // Arrange
            var request = new GetProductQuery() { Id = ProductId };

            // Act
            ProductInfoDto productDto = await SendAsync(request);

            // Assert
            productDto.Should().NotBeNull();
            productDto.Id.Should().Be(ProductId);
        }

        [Fact]
        public void ShouldThrow_WhenProductDoesntExist()
        {
            // Arrange
            var request = new GetProductQuery();

            // Act
            Func<Task<ProductInfoDto>> act = () => SendAsync(new GetProductQuery());

            // Assert
            act.Should().Throw<ValidationException>().And.Error.Message.Should().Be("Product doesn't exist");
        }
    }
}