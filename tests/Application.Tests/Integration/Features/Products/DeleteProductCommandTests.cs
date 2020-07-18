using System;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Features.Products.DeleteProduct;
using Application.Tests.Common.Base;
using Domian.Entities.Products;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Integration.Features.Products
{
	public class DeleteProductCommandTests : RequestTestBase
	{
		[Fact]
		public async Task ShouldDeleteProduct()
		{
			// Arrange & act
			Product productBeforeDelete = await FindAsync<Product>(x => x.Id == ProductId);

			await LoginAsDefaultUserAsync();
			
			await SendAsync(new DeleteProductCommand() {Id = ProductId});

			Product productAfterDelete = await FindAsync<Product>(x => x.Id == ProductId);
			
			// Assert
			productBeforeDelete.Should().NotBeNull();
			productAfterDelete.Should().BeNull();
		}

		[Fact]
		public async Task ShouldThrow_WhenProductDoesntExist()
		{
			// Act
			await LoginAsDefaultUserAsync();
			
			Func<Task> act = () => SendAsync(new DeleteProductCommand());

			// Assert
			act.Should().Throw<ValidationException>().And.Error.Message.Should().Be("Product doesn't exist");
		}
	}
}
