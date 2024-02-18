using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.DAL;

namespace PlantShop.Controllers
{
    public class SucculentController : Controller
    {
        readonly private AppDbContext _context;
        public SucculentController(AppDbContext context)
        {
            _context = context;

        }
        public async Task <IActionResult> Index()
        {
            var succulent = await _context.Product
               .Include(p => p.Categories)
               .Where(p => p.Categories.Name == "Succulent").ToListAsync();
            return View(succulent);
        }
    }
}
