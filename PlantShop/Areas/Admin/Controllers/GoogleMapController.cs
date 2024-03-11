using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.Areas.Admin.ViewModels.GoogleMapVM;
using PlantShop.DAL;
using PlantShop.Helpers;
using PlantShop.Models;

namespace PlantShop.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin")]
    public class GoogleMapController : Controller
    {
        readonly private AppDbContext _context;
        readonly private IFileService _fileService;
        public GoogleMapController(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task <IActionResult> Index()
        {
            List<GoogleMap>? googleMaps = await _context.GoogleMaps.ToListAsync();
            return View(googleMaps);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var googleMaps = await _context.GoogleMaps.FindAsync(id);
            return View(googleMaps);
        }
        public async Task<IActionResult> Create()
        {

            return View(new CreateGoogleMapVM());


        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateGoogleMapVM model)
        {
            if (!ModelState.IsValid) return View(model);


         
            GoogleMap googleMap = new GoogleMap()
            {

               MapUrl=model.MapUrl
            };

            await _context.AddAsync(googleMap);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var googleMap = await _context.GoogleMaps.FirstOrDefaultAsync(h => h.Id == id);

            if (googleMap == null) return NotFound();

            UpdateGoogleMapVM updateGoogleMapVM = new UpdateGoogleMapVM()
            {
                Id = googleMap.Id,
                MapUrl = googleMap.MapUrl
                
            };

            return View(updateGoogleMapVM);
        }
        [HttpPost]

        public async Task<IActionResult> Update(UpdateGoogleMapVM model)
        {

            if (!ModelState.IsValid) return View(model);


            var googleMap = await _context.GoogleMaps.FirstOrDefaultAsync(h => h.Id == model.Id); ;
          

            googleMap.MapUrl = model.MapUrl;

            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var googleMap = await _context.GoogleMaps.FindAsync(id);

            if (googleMap == null) return NotFound();
        
            _context.Remove(googleMap);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
