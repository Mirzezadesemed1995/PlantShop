using Microsoft.AspNetCore.Identity;
using PlantShop.Models;
using PlantShop.Models.Identity;

namespace PlantShop.DbInitilizar
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
                    FullName = "admin",
                    UserName = "admin",
                    Email = "admin@grow.com",
                };

                var password = "876.adDFaFR@#$";
                var confirmPassword = "876.adDFaFR@#$";

                
                var changePasswordResult = await userManager.ChangePasswordAsync(user, confirmPassword, password);

                if (changePasswordResult.Succeeded)
                {
                    
                    var result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                       
                        await userManager.AddToRoleAsync(user, RoleModel.Admin.ToString());
                    }
                    else
                    {
                      
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine(error);
                        }
                    }
                }
                else
                {
                    
                    foreach (var error in changePasswordResult.Errors)
                    {
                        Console.WriteLine(error);
                    }
                }
            }
        }
    }

}
