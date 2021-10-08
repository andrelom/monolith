using Microsoft.EntityFrameworkCore;
using Monolith.Web.Identity.Data.Entities;

namespace Monolith.Web.Identity.Data.Seeders
{
    public static class UserRoleSeeder
    {
        public static void Seed(ModelBuilder builder)
        {
            SeedAdministratorUserRole(builder);
        }

        #region Private Methods: Seeds

        private static void SeedAdministratorUserRole(ModelBuilder builder)
        {
            builder.Entity<UserRole>().HasData(new UserRole()
            {
                RoleId = RoleSeeder.AdministratorRoleId,
                UserId = UserSeeder.AdministratorUserId
            });
        }

        #endregion
    }
}
