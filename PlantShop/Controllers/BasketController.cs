using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.DAL;
using PlantShop.Models;
using PlantShop.Models.Identity;

namespace PlantShop.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class BasketController : Controller
    {

        readonly private UserManager<User> _userManager;
        readonly private AppDbContext _context;
        readonly private IHttpContextAccessor _httpContextAccessor;

        public BasketController(UserManager<User> userManager, AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Add(string id)
        {
            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;

            if (!string.IsNullOrEmpty(username))
            {
                User user = await _userManager.Users
                    .Include(u => u.Baskets)
                    .FirstOrDefaultAsync(u => u.UserName == username);

                var product = await _context.Product.FindAsync(id);

                if (product != null)
                {
                    var userBasket = await _context.Baskets.FirstOrDefaultAsync(b => b.UserId == user.Id);

                    if (userBasket == null)
                    {
                        userBasket = new Basket
                        {
                            UserId = user.Id,
                        };

                        await _context.Baskets.AddAsync(userBasket);
                        await _context.SaveChangesAsync();
                    }

                    var userBaskets = await _context.UserBaskets.FirstOrDefaultAsync(c => c.ProductId.ToString() == id);

                    if (userBaskets == null)
                    {
                        userBaskets = new UserBasket
                        {
                            BasketId = userBasket.Id,
                            ProductId = product.Id,
                            Quantity = 1
                        };

                        await _context.UserBaskets.AddAsync(userBaskets);
                        await _context.SaveChangesAsync();
                    }
                    userBaskets.Quantity++;
                    await _context.SaveChangesAsync();
                }
            }

            return Ok();
        }

    }

}

