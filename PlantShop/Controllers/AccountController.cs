using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlantShop.Areas.Admin.ViewModels.AccountVM;
using PlantShop.Models;
using PlantShop.Models.Identity;

namespace PlantShop.Controllers
{
    public class AccountController : Controller
    {
        readonly private UserManager<User> _userManager;
        readonly private SignInManager<User> _signInManager;
        readonly private RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager , RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (result.Succeeded)
            {
                return RedirectToAction(model.ReturnUrl ?? "~/");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt"); 
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Account is locked out");
            }
            
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }

            return View(model); 
        }
        public async Task<IActionResult> Registration(CreateRegistrationVM model)
        {
            foreach (var roleName in new[] { RoleModel.User.ToString() })
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
                }
            }
           

            var user = new User
            {
               
                FullName= model.FullName,
                Email = model.Email
            };
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.ConfirmPassword,model.Password );

            if (changePasswordResult.Succeeded)
            {

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.ToString());
                        return View(model);
                    }
                  
                }
                else{
                    await _userManager.AddToRoleAsync(user, RoleModel.User.ToString());
                    return RedirectToAction(nameof(Login));
                }

              
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(model);
        }
       [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home"); // Redirect to the home page or any other desired page after logout
        }


    }
}
