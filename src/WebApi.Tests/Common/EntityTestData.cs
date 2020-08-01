using Domian.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using System;
using Application.Common.Constants;
using Application.Common.Interfaces.Security;

namespace WebApi.Tests.Common
{
    public static class EntityTestData
    {
        public static void Seed(DbContext context, IPasswordHasher hasher)
        {
            var role = new Role()
            {
                Id = new Guid("820d5989-0e0c-4eca-b235-d2d02d55f5eb"),
                Name = AuthorizationConstants.Roles.Users
            };
            context.Add(role);

            var user = new User()
            {
                Id = new Guid("7a23912f-e713-4a8f-81f6-17306964dc9a"),
                Name = "user",
                Email = "default@user",
                PasswordHash = hasher.ComputeHash("12345")
            };
            {
                user.UserRoles.Add(new UserRole() { Role = role });

                context.Add(user);
            }

            context.SaveChanges();
        }
    }
}