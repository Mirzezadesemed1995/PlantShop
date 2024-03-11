using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.Areas.Admin.ViewModels.PlantVM;
using PlantShop.Areas.Admin.ViewModels.ShoppingVM;
using PlantShop.DAL;
using PlantShop.Helpers;
using PlantShop.Models;

namespace PlantShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ShoppingController : Controller
    {
        readonly private AppDbContext _context;
        readonly private IFileService _fileService;
        public ShoppingController(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task <IActionResult> Index()
        {
            List<Shopping> shoppings = await _context.Stoppings.ToListAsync();
            return View(shoppings);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var shoppings = await _context.Stoppings.FindAsync(id);
            return View(shoppings);
        }
        public async Task<IActionResult> Create()
        {

            return View();


        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateShoppingVM model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!_fileService.IsImage(model.FormFile))
                ModelState.AddModelError("model.FormFile", "Zehmet olmasa şəkil əlavə edin");
            int maxSize = 60;
            if (!_fileService.CheckSize(model.FormFile, maxSize))
                ModelState.AddModelError("model.FormFile", $"Zehmet olmasa {maxSize} kicik olculu  şəkil əlavə edin");


            var newFile = await _fileService.UploadAsync(model.FormFile);
            Shopping shopping = new Shopping()
            {

                Title = model.Title,
                Description = model.Description,
                FilePath = newFile,
            };

            await _context.AddAsync(shopping);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var shopping = await _context.Stoppings.FirstOrDefaultAsync(h => h.Id == id);

            if (shopping == null) return NotFound();

            UpdatePlantVM updateShoppingVM = new UpdatePlantVM()
            {
                Id = shopping.Id,
                Title = shopping.Title,
                Description = shopping.Description,
                FilePath = shopping.FilePath,
                FormFile = shopping.FormFile,
            };

            return View(updateShoppingVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateShoppingVM model)
        {

            if (!ModelState.IsValid) return View(model);


            var shopping = await _context.Stoppings.FirstOrDefaultAsync(h => h.Id == model.Id); ;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "\\wwwroot\\assets\\img\\", shopping.FilePath);
            _fileService.Delete(path);
            if (model.FormFile != null)
            {
                if (!_fileService.IsImage(model.FormFile))
                    ModelState.AddModelError("model.FormFile", "Zehmet olmasa şəkil əlavə edin");

                if (!_fileService.CheckSize(model.FormFile, 60))
                    ModelState.AddModelError("model.FormFile", "Zehmet olmasa şəkil əlavə edin");


                var newFile = await _fileService.UploadAsync(model.FormFile);
                shopping.FilePath = newFile;
            }

            shopping.Title = model.Title;
            shopping.Description = model.Description;

            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var shopping = await _context.Stoppings.FindAsync(id);

            if (shopping == null) return NotFound();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "\\wwwroot\\assets\\img\\", shopping.FilePath);
            _fileService.Delete(path);
            _context.Remove(shopping);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
