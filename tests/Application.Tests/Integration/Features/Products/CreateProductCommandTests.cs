using System;
using System.Threading.Tasks;
using Application.Features.Products.CreateProduct;
using Application.Tests.Common.Base;
using Domian.Entities;
using Domian.Entities.Products;
using Domian.ValueObjects;
using FluentAssertions;
using Infrastructure.DataAccess;
using Xunit;

namespace Application.Tests.Integration.Features.Products
{
	public class CreateProductCommandTests : RequestTestBase
	{
		private readonly Guid _brandId = new Guid("794cdf88-3bda-4b8a-a70e-ee4703baa330");
		private readonly Guid _categoryId = new Guid("237ccfc3-2bc7-4b93-9e67-e32e53ec615a");
		private readonly Guid _manufacturerId = new Guid("a3590854-dca1-4e7b-9d0c-671dc29209e3");
		private readonly Guid _currencyId = new Guid("db8e7a46-f1fd-4b0d-8b9b-5cdf8fea1b5f");

		protected override async Task SeedContextAsync(AppDbContext context)
		{
			await base.SeedContextAsync(context);

			await context.AddAsync(new Brand
			{
				Id = _brandId,
				Name = "Brand"
			});

			await context.AddAsync(new Category
			{
				Id = _categoryId,
				Name = "Name"
			});

			await context.AddAsync(new Country
			{
				Id = new Guid("8ecbb8f1-768e-49f4-a50a-828db7ddc504"),
				Continent = "Continent",
				Name = "Name"
			});

			await context.AddAsync(new City
			{
				Id = new Guid("7cda3f58-b760-4d67-b70d-2eeb56a6115a"),
				CountryId = new Guid("8ecbb8f1-768e-49f4-a50a-828db7ddc504"),
				Name = "City"
			});

			await context.AddAsync(new Manufacturer
			{
				Id = _manufacturerId,
				HeadOffice = new Address
				{
					CityId = new Guid("7cda3f58-b760-4d67-b70d-2eeb56a6115a"),
					StreetName = "StreetName",
					ZipCode = "12345"
				},
				ContactPhone = "1",
				HomePageUrl = "HomePageUrl",
				OrganizationName = "OrganizationName"
			});

			await context.AddAsync(new Currency
			{
				Id = _currencyId,
				Code = "RUB"
			});
		}

		[Fact]
		public async Task ShouldCreateProduct()
		{
			// Arrange
			var request = new CreateProductCommand
			{
				Name = "Product",
				BrandId = _brandId,
				CategoryId = _categoryId,
				ManufacturerId = _manufacturerId,
				PriceCurrencyId = _currencyId,
				Description = "Description",
				PriceAmount = 2
			};

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
	}
}
