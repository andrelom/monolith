using System;
using Microsoft.EntityFrameworkCore;
using Monolith.Web.Identity.Data.Entities;

namespace Monolith.Web.Identity.Data.Seeders
{
    public static class RoleSeeder
    {
        public static readonly Guid AdministratorRoleId = Guid.NewGuid();

        public static void Seed(ModelBuilder builder)
        {
            SeedAdministratorRole(builder);
        }

        #region Private Methods: Seeds

        private static void SeedAdministratorRole(ModelBuilder builder)
        {
            var id = AdministratorRoleId;
            var name = Roles.Administrator;

            builder.Entity<Role>().HasData(new Role
            {
                Id = id,
                ConcurrencyStamp = id.ToString(),
                Name = name,
                NormalizedName = name.ToUpperInvariant()
            });
        }

        #endregion
    }
}
