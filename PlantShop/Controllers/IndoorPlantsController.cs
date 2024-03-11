using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.DAL;

namespace PlantShop.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class IndoorPlantsController : Controller
    {
        readonly private AppDbContext _context;
        public IndoorPlantsController(AppDbContext context)
        {
            _context = context;

        }
        public async Task <IActionResult> Index()
        {
            var indoor = await _context.Product
               .Include(p => p.Categories)
               .Where(p => p.Categories.Name == "Indoor Plants").ToListAsync();
            return View(indoor);
        }
    }
}
