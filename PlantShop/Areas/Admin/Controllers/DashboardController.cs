using Microsoft.AspNetCore.Mvc;

namespace PlantShop.Areas.Admin.Controllers
{
    [Area("admin")]
    public class DashboardController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
