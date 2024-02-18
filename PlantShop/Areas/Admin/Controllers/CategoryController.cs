using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.Areas.Admin.ViewModels.CategoryVM;
using PlantShop.DAL;
using PlantShop.Helpers;
using PlantShop.Models;
using System.Security.Cryptography.X509Certificates;

namespace PlantShop.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CategoryController : Controller
    {

        readonly private AppDbContext _context;
        readonly private IFileService _fileService;
        public CategoryController(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {

            List<Category> categories = await _context.Category.ToListAsync();


            return View(categories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        public IActionResult Create()
        => View();
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM model)
        {
            if (!ModelState.IsValid) return View(model);
            if (!_fileService.IsImage(model.formFile))
                ModelState.AddModelError("model.FormFile", "Zehmet olmasa şəkil əlavə edin");
            int maxSize = 60;
            if (!_fileService.CheckSize(model.formFile, maxSize))
                ModelState.AddModelError("model.FormFile", $"Zehmet olmasa {maxSize} kicik olculu  şəkil əlavə edin");

            var newFile = await _fileService.UploadAsync(model.formFile);
            Category category = new Category()
            {
                Name = model.Name,
                FilePath = newFile,
            };

            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null) return NotFound();
            UpdateCategoryVM updateCategoryVM = new UpdateCategoryVM()
            {
                Id = category.Id,
                Name = category.Name,
                FilePath = category.FilePath,
                formFile = category.formFile

            };
            return View(updateCategoryVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryVM model)
        {
            var category = await _context.Category.FindAsync(model.Id);
            if (category == null) return BadRequest();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "\\wwwroot\\assets\\img\\", model.FilePath);

            _fileService.Delete(path);
            
            if (model.formFile != null)
            {
                if (!_fileService.IsImage(model.formFile))
                    ModelState.AddModelError("model.FormFile", "Zehmet olmasa şəkil əlavə edin");
                int maxSize = 60;
                if (!_fileService.CheckSize(model.formFile, maxSize))
                    ModelState.AddModelError("model.FormFile", $"Zehmet olmasa {maxSize} kicik olculu  şəkil əlavə edin");

                var newFile = await _fileService.UploadAsync(model.formFile);
            }
            category.Name= model.Name;
            _context.Category.Update(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null) return NotFound();

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();  
            return RedirectToAction(nameof(Index));
        }

    }
}
