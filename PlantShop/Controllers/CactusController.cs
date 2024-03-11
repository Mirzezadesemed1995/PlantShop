using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.DAL;

namespace PlantShop.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class CactusController : Controller
    {
        readonly private AppDbContext _context;

        public CactusController(AppDbContext context)
        {
            _context = context;

        }
        public async Task <IActionResult> Index()
        {
            var cactus=await _context.Product
                .Include(p=>p.Categories)
                .Where(p=>p.Categories.Name=="cactus").ToListAsync();
            return View(cactus);
        }
    }
}
