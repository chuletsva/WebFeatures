using System;
using System.Threading.Tasks;
using Application.Common.Constants;
using AutoFixture;
using Domian.Entities;
using Domian.Entities.Accounts;
using Domian.Entities.Products;
using Domian.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Tests.Common.Helpers
{
    public static class EntityTestData
    {
        public static readonly Guid BrandId = new Guid("2180f19a-c577-4a2b-b9e1-c1371d38c6a0");
        public static readonly Guid CategoryId = new Guid("c57eff72-3ef3-480c-8fb2-c1ea96116cb7");
        public static readonly Guid CountryId = new Guid("9fde6181-016c-4de9-b18a-36f339a7d5c3");
        public static readonly Guid CityId = new Guid("ad239752-3954-4a66-b8c9-5c2756f3d72c");
        public static readonly Guid ManufacturerId = new Guid("3b0e08dd-529b-47ca-a1b0-d781a729eff9");
        public static readonly Guid CurrencyId = new Guid("ba0e193d-47f7-4abd-a780-f9d4ff97b9c1");
        public static readonly Guid ProductId = new Guid("d5a4a565-765b-4aa7-8bd4-edb4e2100933");
        public static readonly Guid UserId = new Guid("50d5a465-254e-4779-bf5a-169ed2ece8b4");

        public static async Task SeedContextAsync(DbContext context)
        {
            var fixture = new Fixture();
            {
                fixture.Customizations.Add(new EntitySpecimenBuilder());
            }

            Role role = fixture.Build<Role>()
               .With(x => x.Name, AuthorizationConstants.Roles.Users)
               .Create();
            {
                await context.AddAsync(role);
            }

            User user = fixture.Build<User>()
               .With(x => x.Id, UserId)
               .With(x => x.Email, "default@user")
               .Create();
            {
                await context.AddAsync(user);
            }

            Brand brand = fixture.Build<Brand>()
               .With(x => x.Id, BrandId)
               .Create();
            {
                await context.AddAsync(brand);
            }

            Category category = fixture.Build<Category>()
               .With(x => x.Id, CategoryId)
               .Create();
            {
                await context.AddAsync(category);
            }

            Country country = fixture.Build<Country>()
               .With(x => x.Id, CountryId)
               .Create();
            {
                await context.AddAsync(country);
            }

            City city = fixture.Build<City>()
               .With(x => x.Id, CityId)
               .With(x => x.CountryId, country.Id)
               .Create();
            {
                await context.AddAsync(city);
            }

            Manufacturer manufacturer = fixture.Build<Manufacturer>()
               .With(x => x.Id, ManufacturerId)
               .With(x => x.HeadOffice, () =>
                    fixture.Build<Address>()
                       .With(x => x.CityId, CityId)
                       .Create())
               .Create();
            {
                await context.AddAsync(manufacturer);
            }

            Currency currency = fixture.Build<Currency>()
               .With(x => x.Id, CurrencyId)
               .Create();
            {
                await context.AddAsync(currency);
            }

            Product product = fixture.Build<Product>()
               .With(x => x.Id, ProductId)
               .With(x => x.Price, () =>
                    fixture.Build<Money>()
                       .With(x => x.CurrencyId, CurrencyId)
                       .Create())
               .With(x => x.BrandId, BrandId)
               .With(x => x.CategoryId, CategoryId)
               .With(x => x.ManufacturerId, ManufacturerId)
               .Create();
            {
                await context.AddAsync(product);
            }
        }
    }
}