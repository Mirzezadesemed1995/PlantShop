using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.Areas.Admin.ViewModels.GardenVM;
using PlantShop.Areas.Admin.ViewModels.PlantVM;
using PlantShop.DAL;
using PlantShop.Helpers;
using PlantShop.Models;

namespace PlantShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PlantController : Controller
    {
        readonly private AppDbContext _context;
        readonly private IFileService _fileService;

        public PlantController(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            List<Plant> plants = await _context.Plants.ToListAsync();
            return View(plants);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var plants = await _context.Plants.FindAsync(id);
            return View(plants);
        }
        public async Task<IActionResult> Create()
        {

            return View();


        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePlantVM model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!_fileService.IsImage(model.FormFile))
                ModelState.AddModelError("model.FormFile", "Zehmet olmasa şəkil əlavə edin");
            int maxSize = 60;
            if (!_fileService.CheckSize(model.FormFile, maxSize))
                ModelState.AddModelError("model.FormFile", $"Zehmet olmasa {maxSize} kicik olculu  şəkil əlavə edin");


            var newFile = await _fileService.UploadAsync(model.FormFile);
            Plant plant = new Plant()
            {

                Title = model.Title,
                Description = model.Description,
                FilePath = newFile,
            };

            await _context.AddAsync(plant);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var plant = await _context.Plants.FirstOrDefaultAsync(h => h.Id == id);

            if (plant == null) return NotFound();

            UpdatePlantVM updatePlantVM = new UpdatePlantVM()
            {
                Id = plant.Id,
                Title = plant.Title,
                Description = plant.Description,
                FilePath = plant.FilePath,
                FormFile = plant.FormFile,
            };

            return View(updatePlantVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdatePlantVM model)
        {

            if (!ModelState.IsValid) return View(model);


            var plant = await _context.Plants.FirstOrDefaultAsync(h => h.Id == model.Id); ;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "\\wwwroot\\assets\\img\\", plant.FilePath);
            _fileService.Delete(path);
            if (model.FormFile != null)
            {
                if (!_fileService.IsImage(model.FormFile))
                    ModelState.AddModelError("model.FormFile", "Zehmet olmasa şəkil əlavə edin");

                if (!_fileService.CheckSize(model.FormFile, 60))
                    ModelState.AddModelError("model.FormFile", "Zehmet olmasa şəkil əlavə edin");


                var newFile = await _fileService.UploadAsync(model.FormFile);
                plant.FilePath = newFile;
            }

            plant.Title = model.Title;
            plant.Description = model.Description;

            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var plant = await _context.Plants.FindAsync(id);

            if (plant == null) return NotFound();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "\\wwwroot\\assets\\img\\", plant.FilePath);
            _fileService.Delete(path);
            _context.Remove(plant);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

