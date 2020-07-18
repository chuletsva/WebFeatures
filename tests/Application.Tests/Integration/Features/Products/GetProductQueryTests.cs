using System;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Features.Products.GetProduct;
using Application.Tests.Common.Base;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Integration.Features.Products
{
	public class GetProductQueryTests : RequestTestBase
	{
		[Fact]
		public async Task ShouldReturnProduct()
		{
			// Arrange
			var request = new GetProductQuery() {Id = ProductId};
			
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
			Func<Task<ProductInfoDto>> act = () => SendAsync(request);
			
			// Assert
			act.Should().Throw<ValidationException>().And.Error.Message.Should().Be("Product doesn't exist");
		}
	}
}