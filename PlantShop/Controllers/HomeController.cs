using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.DAL;
using PlantShop.Models;
using PlantShop.ViewModels;

namespace PlantShop.Controllers
{
    public class HomeController : Controller
    {
        readonly private AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context; 
        }
        public async Task<IActionResult> Index()
        {
            var product = await _context.Product.Take(4).ToListAsync();
            var header = await _context.Headers.ToListAsync();
            HomeIndexVM vm = new HomeIndexVM()
            {
                Products = product,
                Headers = header
            };
                
             return View(vm);

        }
    }
}
