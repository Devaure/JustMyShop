using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjPermManage.constants;

namespace ProjPermManage.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedBasicUserAsync(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new IdentityUser
            {
                UserName = "default@gmail.com",
                Email = "default@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true

            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                //je cherche le userManager avec L'email par defaut du defaultUser
                var user = await userManager.FindByEmailAsync(defaultUser.Email);

                //si pas d'utilisateur userManager
                if (user == null)
                {
                    //je créer l'utilisateur userManager en lui donnant les information de base 
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());

                }
            }
        }

        public static async Task SeedSuperAdminAsync(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new IdentityUser
            {
                UserName = "superadmin@gmail.com",
                Email = "superadmin@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true

            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                //je cherche le userManager avec L'email par defaut du defaultUser
                var user = await userManager.FindByEmailAsync(defaultUser.Email);

                //si pas d'utilisateur userManager
                if (user == null)
                {
                    //je créer l'utilisateur userManager en lui donnant les information de base 
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
        }

        private async static Task SeedClaimsForSuperAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("SuperAdmin");
            await roleManager.AddPermissionClaim(adminRole, "Products");
        }

        public async static Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager,
            IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionForModule(module);

            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }
    }
}
