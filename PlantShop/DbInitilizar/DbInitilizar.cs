
using Microsoft.AspNetCore.Identity;
using PlantShop.Models;
using PlantShop.Models.Identity;

namespace PlantShop.DbInitializer
{
    public class DbInitializer
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(RoleModel)))
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString(),
                    });
                }
            }

            if (await userManager.FindByNameAsync("admin") == null)
            {
                var user = new User
                {
                    UserName = "admin",
                    Email = "admin@grow.com",
                };

                var password = GenerateRandomPassword(); // Consider generating a secure password

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, RoleModel.Admin.ToString());
                }
                else
                {
                    // Handle errors appropriately
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
            }
        }

        private static string GenerateRandomPassword()
        {
            
            return "876.adDFaFR@#$";
        }
    }
}
