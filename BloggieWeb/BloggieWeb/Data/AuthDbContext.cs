using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BloggieWeb.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //seed roles(uses,admin, superadmin)

            var adminRoleId = "9cb32ff9-1dfa-4cbb-8048-06614d5511be";
            var superAdminRoleId = "99798969-8f51-48b6-9b1e-d53d1129c29c";
            var userRoleId = "3dffae32-65a7-413b-a235-bb1a216c3988";


            var roles = new List<IdentityRole>
            {
               new IdentityRole
               {
                   Name= "Admin",
                   NormalizedName = "Admin",
                   Id =adminRoleId,
                   ConcurrencyStamp = adminRoleId
               },

              new  IdentityRole{
                  Name="SuperAdmin",
                  NormalizedName = "SuperAdmin",
                  Id= superAdminRoleId,
                  ConcurrencyStamp = superAdminRoleId
                },
              new IdentityRole
              {
                  Name="User",
                  NormalizedName="User",
                  Id = userRoleId,
                  ConcurrencyStamp = userRoleId
              }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            //see superadminuser

            var superAdminId = "20226c05-09ca-4d37-9cca-ae94e4f3474a";

            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@bloggie.com",
                Email = "superadmin@bloggie.com",
                NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
                NormalizedUserName = "superadmin@bloggie.com".ToUpper(),
                Id = superAdminId
            };
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "Superadmin@123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);
            //add all role to superadminuser
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId,

                },
                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId,

                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId,

                },
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);

        }
    }
}
