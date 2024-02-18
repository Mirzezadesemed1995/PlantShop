using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.DAL;

namespace PlantShop.Controllers
{
    public class BonsaiController : Controller
    {
        readonly private AppDbContext _context;
        public BonsaiController(AppDbContext context)
        {
            _context = context;

        }
        public async Task <IActionResult> Index()
        {
            var bonsai = await _context.Product
              .Include(p => p.Categories)
              .Where(p => p.Categories.Name == "Bonsai").ToListAsync();
            return View(bonsai);
        }
    }
}
