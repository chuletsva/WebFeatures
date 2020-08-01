using Application.Constants;
using Application.Interfaces.Security;
using Domian.Entities;
using Domian.Entities.Accounts;
using Domian.Entities.Products;
using Domian.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess
{
    public static class AppDbContextSeed
    {
        public static async Task Seed(AppDbContext context, IPasswordHasher hasher)
        {
            if (await context.Users.AnyAsync()) return;

            await SeedData(context, hasher);

            await CreateHangfireUser(context);
        }

        private static async Task SeedData(AppDbContext context, IPasswordHasher hasher)
        {
            await context.Users.AddRangeAsync(
                new User()
                {
                    Id = new Guid("f9fb9878-6236-44d0-9941-21417ec8d5f6"),
                    Name = "user",
                    Email = "user@mail.com",
                    PasswordHash = hasher.ComputeHash("12345")
                });

            await context.Brands.AddRangeAsync(
                new Brand()
                {
                    Id = new Guid("873d861e-14e1-4757-91f4-cae6beba4010"),
                    Name = "name"
                });

            await context.Products.AddRangeAsync(
                new Product()
                {
                    Name = "product",
                    Description = "description",
                    ManufacturerId = new Guid("0ea02742-3566-416f-94e5-bc9d878769f3"),
                    BrandId = new Guid("873d861e-14e1-4757-91f4-cae6beba4010"),
                    CreatedAt = DateTime.UtcNow,
                    CreatedById = new Guid("f9fb9878-6236-44d0-9941-21417ec8d5f6"),
                    Price = new Money()
                    {
                        Amount = 1,
                        CurrencyId = new Guid("d39e9efe-07af-40d7-bb5e-9904f4bc0fc2")
                    }
                });

            await context.Manufacturers.AddRangeAsync(
                new Manufacturer()
                {
                    Id = new Guid("0ea02742-3566-416f-94e5-bc9d878769f3"),
                    OrganizationName = "manufacturer",
                    HeadOffice = new Address()
                    {
                        CityId = new Guid("685e8905-bad5-4767-ac2c-2bae7ead13be"),
                        StreetName = "streetname",
                        ZipCode = "12345"
                    }
                });

            await context.Cities.AddRangeAsync(
                new City()
                {
                    Id = new Guid("685e8905-bad5-4767-ac2c-2bae7ead13be"),
                    CountryId = new Guid("46bd74a1-e141-45e2-aaec-ba3616d95819"),
                    Name = "city"
                });

            await context.Countries.AddRangeAsync(
                new Country()
                {
                    Id = new Guid("46bd74a1-e141-45e2-aaec-ba3616d95819"),
                    Continent = "continent",
                    Name = "country"
                });

            await context.Currencies.AddRangeAsync(
                new Currency()
                {
                    Id = new Guid("d39e9efe-07af-40d7-bb5e-9904f4bc0fc2"),
                    Code = "RUB"
                });

            await context.Roles.AddRangeAsync(
                new Role()
                {
                    Name = AuthorizationConstants.Roles.Users,
                    Description = "Пользователи"
                });

            await context.SaveChangesAsync(acceptAllChangesOnSuccess: true);
        }

        private static async Task CreateHangfireUser(AppDbContext context)
        {
            string db = context.Database.GetDbConnection().Database;

            string sql =
                $@"DO
                $$
                BEGIN
                    IF NOT EXISTS ( 
                        SELECT FROM pg_catalog.pg_roles
                        WHERE rolname = 'hangfire') 
                    THEN
                        CREATE ROLE hangfire WITH 
                        LOGIN 
                        PASSWORD 'hangfire';
                    END IF;

                    GRANT ALL PRIVILEGES ON DATABASE {db} TO hangfire;
                END
                $$;";

            await context.Database.ExecuteSqlRawAsync(sql);
        }
    }
}