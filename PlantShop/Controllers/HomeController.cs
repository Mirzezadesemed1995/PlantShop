

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.DAL;
using PlantShop.Models;
using PlantShop.Models.Identity;
using PlantShop.ViewModels;

namespace PlantShop.Controllers
{
    public class HomeController : Controller
    {
        readonly private AppDbContext _context;
        readonly private  IHttpContextAccessor _httpContextAccessor;
        readonly private UserManager<User> _userManager;    
        public HomeController(AppDbContext context ,  IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            
            _context = context; 
            _userManager= userManager;  
            _httpContextAccessor= httpContextAccessor;  
        }
        public async Task<IActionResult> Index()
               
        {
            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;



            User? user = await _userManager.Users
				.Include(u => u.Baskets)
				.FirstOrDefaultAsync(u => u.UserName == username);
            
			var product = await _context?.Product.ToListAsync();
            var pr = await _context?.Product.ToListAsync();
            var header = await _context?.Headers.ToListAsync();
            var basket = await _context?.Baskets?.ToListAsync();
            var userBasket = await _context?.UserBaskets.ToListAsync();  
			HomeIndexVM vm = new HomeIndexVM()
            {
               
				UserBasket=userBasket,
                User=user,
                Products = product ,
                Headers = header,
                Baskets=basket,
            };


            return View(vm);

        }
 
    }
      
}
