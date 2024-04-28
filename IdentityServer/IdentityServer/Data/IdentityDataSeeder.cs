using Microsoft.AspNetCore.Identity;
using IdentityServer.Models;

namespace IdentityServer.Data
{
    public class IdentityDataSeeder
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityDataSeeder(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAllAsync()
        {
            await SeedRoles();
            await SeedUsers();
        }

        private async Task SeedRoles()
        {
            if (!await _roleManager.RoleExistsAsync(ApplicationRole.Admin.Name))
                await _roleManager.CreateAsync(ApplicationRole.Admin);

            if (!await _roleManager.RoleExistsAsync(ApplicationRole.User.Name))
                await _roleManager.CreateAsync(ApplicationRole.User);
        }

        private async Task SeedUsers()
        {
            if (await _userManager.FindByEmailAsync("test@test.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "test",
                    //FirstName = "test",
                    //LastName = "test",
                    Email = "test@test.com",
                };

                var result = await _userManager.CreateAsync(user, "123456");

                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(user, ApplicationRole.Admin.Name);
            }

            if (await _userManager.FindByEmailAsync("test2@test.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "test2",
                    //FirstName = "test",
                    //LastName = "test",
                    Email = "test2@test.com"
                };

                var result = await _userManager.CreateAsync(user, "123456");

                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(user, ApplicationRole.User.Name);
            }
        }
    }
}
