using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Monolith.Web.Identity.Data.Entities;

namespace Monolith.Web.Identity.Data.Seeders
{
    public static class UserSeeder
    {
        public static readonly Guid AdministratorUserId = Guid.NewGuid();

        public static void Seed(ModelBuilder builder)
        {
            SeedAdministratorUser(builder);
        }

        #region Private Methods: Seeds

        private static void SeedAdministratorUser(ModelBuilder builder)
        {
            const string name = "monolith";
            const string email = "monolith@monolith.com";
            const string password = "@QwTXb6g2kt2HvDx";

            var id = AdministratorUserId;
            var hasher = new PasswordHasher<User>();

            var user = new User
            {
                Id = id,
                ConcurrencyStamp = id.ToString(),
                UserName = name,
                NormalizedUserName = name.ToUpperInvariant(),
                Email = email,
                NormalizedEmail = email.ToUpperInvariant(),
                SecurityStamp = id.ToString(),
                EmailConfirmed = true
            };

            user.PasswordHash = hasher.HashPassword(user, password);

            builder.Entity<User>().HasData(user);
        }

        #endregion
    }
}
