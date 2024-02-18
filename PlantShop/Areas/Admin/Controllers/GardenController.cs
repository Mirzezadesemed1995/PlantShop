using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.Areas.Admin.ViewModels.GardenVM;
using PlantShop.Areas.Admin.ViewModels.HeaderVM;
using PlantShop.DAL;
using PlantShop.Helpers;
using PlantShop.Models;

namespace PlantShop.Areas.Admin.Controllers
{
    [Area("admin")]
    public class GardenController : Controller
    {
        readonly private AppDbContext _context;
        readonly private IFileService _fileService;

        public GardenController(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            List<Garden> gardens = await _context.Gardens.ToListAsync();
            return View(gardens);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var gardens = await _context.Gardens.FindAsync(id);
            return View(gardens);
        }
        public async Task<IActionResult> Create()
        {
           
                return View();
            
               
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateGardenVM model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!_fileService.IsImage(model.FormFile))
                ModelState.AddModelError("model.FormFile", "Zehmet olmasa şəkil əlavə edin");
            int maxSize = 60;
            if (!_fileService.CheckSize(model.FormFile, maxSize))
                ModelState.AddModelError("model.FormFile", $"Zehmet olmasa {maxSize} kicik olculu  şəkil əlavə edin");


            var newFile = await _fileService.UploadAsync(model.FormFile);
            Garden garden = new Garden()
            {

                Title = model.Title,
                Description = model.Description,
                FilePath = newFile,
            };

            await _context.AddAsync(garden);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var garden = await _context.Gardens.FirstOrDefaultAsync(h => h.Id == id);

            if (garden == null) return NotFound();

            UpdateGardenVM updateGardenVM = new UpdateGardenVM()
            {
                Id = garden.Id,
                Title = garden.Title,
                Description = garden.Description,
                FilePath = garden.FilePath,
                FormFile = garden.FormFile,
            };

            return View(updateGardenVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateGardenVM model)
        {

            if (!ModelState.IsValid) return View(model);


            var garden = await _context.Gardens.FirstOrDefaultAsync(h => h.Id == model.Id); ;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "\\wwwroot\\assets\\img\\", garden.FilePath);
            _fileService.Delete(path);
            if (model.FormFile != null)
            {
                if (!_fileService.IsImage(model.FormFile))
                    ModelState.AddModelError("model.FormFile", "Zehmet olmasa şəkil əlavə edin");

                if (!_fileService.CheckSize(model.FormFile, 60))
                    ModelState.AddModelError("model.FormFile", "Zehmet olmasa şəkil əlavə edin");


                var newFile = await _fileService.UploadAsync(model.FormFile);
                garden.FilePath = newFile;
            }

            garden.Title = model.Title;
            garden.Description = model.Description;

            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var garden = await _context.Gardens.FindAsync(id);

            if (garden == null) return NotFound();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "\\wwwroot\\assets\\img\\", garden.FilePath);
            _fileService.Delete(path);
            _context.Remove(garden);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
