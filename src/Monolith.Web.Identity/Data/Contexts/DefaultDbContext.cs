using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Monolith.Web.Identity.Data.Entities;
using Monolith.Web.Identity.Data.Seeders;

namespace Monolith.Web.Identity.Data.Contexts
{
    public class DefaultDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            RoleSeeder.Seed(builder);
            UserSeeder.Seed(builder);
            UserRoleSeeder.Seed(builder);
        }
    }
}
