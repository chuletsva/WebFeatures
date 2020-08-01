using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Features.Products.CreateProduct;
using Application.Tests.Common.Base;
using Application.Tests.Common.Helpers;
using AutoFixture;
using Domian.Entities.Products;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Integration.Features.Products
{
	using static EntityTestData;
	
	public class CreateProductCommandTests : RequestTestBase
	{
		[Fact]
		public async Task ShouldCreateProduct()
		{
			// Arrange
			var request = new Fixture()
			   .Build<CreateProductCommand>()
			   .With(x => x.BrandId, BrandId)
			   .With(x => x.CategoryId, CategoryId)
			   .With(x => x.ManufacturerId, ManufacturerId)
			   .With(x => x.PriceCurrencyId, CurrencyId)
			   .Create();

			// Act		
			Guid userId = await LoginAsDefaultUserAsync();

			Guid productId = await SendAsync(request);

			Product product = await FindAsync<Product>(x => x.Id == productId);

			// Assert
			product.Should().NotBeNull();
			product.Id.Should().Be(productId);
			product.Name.Should().Be(request.Name);
			product.BrandId.Should().Be(request.BrandId);
			product.CategoryId.Should().Be(request.CategoryId);
			product.ManufacturerId.Should().Be(request.ManufacturerId);
			product.Price.Should().NotBeNull();
			product.Price.CurrencyId.Should().Be(request.PriceCurrencyId);
			product.Price.Amount.Should().Be(request.PriceAmount);
			product.CreatedById.Should().Be(userId);
			product.CreatedAt.Date.Should().Be(DateTime.UtcNow.Date);
		}

		[Fact]
		public async Task ShouldThrow_WhenInvalidProduct()
		{
			// Act
			await LoginAsDefaultUserAsync();
			
			Func<Task<Guid>> act = () => SendAsync(new CreateProductCommand());
			
			// Assert
			act.Should().Throw<ValidationException>().And.Error.Errors.Should().NotBeEmpty();
		}
	}
}
