using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.DAL;
using PlantShop.Models;
using PlantShop.Models.Identity;
using System.Net;

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
        [HttpPost]
        public async Task<IActionResult> Add(int id, int  quantity)
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
                    var basket = await _context.Baskets.FirstOrDefaultAsync(b => b.UserId == user.Id);

                    if (basket == null)
                    {
                        basket = new Basket
                        {
                            UserId = user.Id,
                        };

                        await _context.Baskets.AddAsync(basket);
                        await _context.SaveChangesAsync();
                        var userBaskets = await _context.UserBaskets.FirstOrDefaultAsync(c => c.ProductId == id && c.BasketId ==basket.Id);

                       
                            userBaskets = new UserBasket
                            {
                                BasketId = basket.Id,
                                ProductId = product.Id,
                                Quantity = quantity
                            };

                            await _context.UserBaskets.AddAsync(userBaskets);
                            await _context.SaveChangesAsync();
                        
                    }
                    else
                    {
                        var userBaskets = await _context.UserBaskets.FirstOrDefaultAsync(c => c.ProductId == id && c.BasketId==basket.Id);

                        if (userBaskets == null)
                        {
                            userBaskets = new UserBasket
                            {
                                BasketId = basket.Id,
                                ProductId = product.Id,
                                Quantity = quantity
                            };

                            await _context.UserBaskets.AddAsync(userBaskets);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            userBaskets.Quantity += quantity;
                            await _context.SaveChangesAsync();
                        }
                    }

                   
                  
                }
            }

			return StatusCode((int)HttpStatusCode.Created);
		}

    }

}

