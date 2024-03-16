using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.DAL;
using PlantShop.Models;
using PlantShop.ViewModels;

namespace PlantShop.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class ShopController : Controller
    {
        readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> product = await _context.Product.Include(p => p.Categories).ToListAsync();
            return View(product);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Product.FindAsync(id);
            var files = await _context.Files.ToListAsync();
            var category = await _context.Category.Include(a => a.Product).FirstOrDefaultAsync();
            if (product == null) return BadRequest();
            ShopDetailsVM vm = new ShopDetailsVM()
            {
                Files = files,
                Product = product,
                Category = category
            };

            return View(vm);
        }


    }
}
