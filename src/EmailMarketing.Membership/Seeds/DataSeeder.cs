using EmailMarketing.Membership.Constants;
using EmailMarketing.Membership.Entities;
using EmailMarketing.Membership.Enums;
using EmailMarketing.Membership.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace EmailMarketing.Membership.Seeds
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(
            ApplicationUserManager userManager,
            ApplicationRoleManager roleManager)
        {
            await SeedRolesAsync(roleManager);
            await SeedSuperAdminAsync(userManager);
        }

        private static async Task SeedRolesAsync(ApplicationRoleManager roleManager)
        {
            var roles = new[]
            {
                ConstantsUserRoleName.SuperAdmin,
                ConstantsUserRoleName.Admin,
                ConstantsUserRoleName.Member
            };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole(roleName)
                    {
                        Status = EnumApplicationRoleStatus.SuperAdmin,
                        Created = DateTime.UtcNow,
                        IsActive = true,
                        IsDeleted = false
                    });
                }
            }
        }

        private static async Task SeedSuperAdminAsync(ApplicationUserManager userManager)
        {
            const string email = "admin@mailto.com";
            const string password = "Admin@1234";

            if (await userManager.FindByEmailAsync(email) != null)
                return;

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = "Super Admin",
                EmailConfirmed = true,
                Status = EnumApplicationUserStatus.SuperAdmin,
                PasswordChangedCount = 1,
                IsActive = true,
                IsDeleted = false,
                IsBlocked = false,
                Created = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, ConstantsUserRoleName.SuperAdmin);
            }
        }
    }
}
