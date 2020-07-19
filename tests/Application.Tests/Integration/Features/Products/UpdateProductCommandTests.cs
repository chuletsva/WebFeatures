using System;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Features.Products.UpdateProduct;
using Application.Tests.Common.Base;
using Application.Tests.Common.Helpers;
using AutoFixture;
using Domian.Entities.Products;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Integration.Features.Products
{
	using static EntityTestData;
	
	public class UpdateProductCommandTests : RequestTestBase
	{
		[Fact]
		public async Task ShouldUpdateProduct()
		{
			// Arrange
			var request = new Fixture()
			   .Build<UpdateProductCommand>()
			   .With(x => x.Id, ProductId)
			   .With(x => x.BrandId, BrandId)
			   .With(x => x.CategoryId, CategoryId)
			   .With(x => x.ManufacturerId, ManufacturerId)
			   .With(x => x.PriceCurrencyId, CurrencyId)
			   .Without(x => x.PictureId)
			   .Create();

			// Act		
			Guid userId = await LoginAsDefaultUserAsync();

			await SendAsync(request);

			Product product = await FindAsync<Product>(x => x.Id == request.Id);

			// Assert
			product.Should().NotBeNull();
			product.Id.Should().Be(request.Id);
			product.Name.Should().Be(request.Name);
			product.BrandId.Should().Be(request.BrandId);
			product.CategoryId.Should().Be(request.CategoryId);
			product.ManufacturerId.Should().Be(request.ManufacturerId);
			product.Price.Should().NotBeNull();
			product.Price.CurrencyId.Should().Be(request.PriceCurrencyId);
			product.Price.Amount.Should().Be(request.PriceAmount);
			product.UpdatedById.Should().Be(userId);
			product.UpdatedAt?.Date.Should().Be(DateTime.UtcNow.Date);
		}

		[Fact]
		public async Task ShouldThrow_WhenInvalidProduct()
		{
			// Act
			await LoginAsDefaultUserAsync();
			
			Func<Task> act = () => SendAsync(new UpdateProductCommand());
			
			// Assert
			act.Should().Throw<ValidationException>().And.Error.Errors.Should().NotBeEmpty();
		}
	}
}
