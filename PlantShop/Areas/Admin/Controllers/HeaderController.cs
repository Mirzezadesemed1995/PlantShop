using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PlantShop.Areas.Admin.ViewModels.HeaderVM;
using PlantShop.DAL;
using PlantShop.Helpers;
using PlantShop.Models;

namespace PlantShop.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin")]
    public class HeaderController : Controller
    {
        readonly private AppDbContext _context;
        readonly private IFileService _fileService;

        public HeaderController(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            List<Header> headers = await _context.Headers.ToListAsync();

            return View(headers);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var headers = await _context.Headers.FindAsync(id);
            return View(headers);
        }

        public async Task<IActionResult> Create()
        {
            List<Header> headers = await _context.Headers.ToListAsync();
            if (headers!=null) return NotFound();   

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateHeaderVM model)
        {
            if (!ModelState.IsValid) return View(model);
            
            if (!_fileService.IsImage(model.FormFile))
                ModelState.AddModelError("model.FormFile", "Zehmet olmasa şəkil əlavə edin");
            int maxSize = 60;
            if (!_fileService.CheckSize(model.FormFile,maxSize))
                ModelState.AddModelError("model.FormFile", $"Zehmet olmasa {maxSize} kicik olculu  şəkil əlavə edin");


            var newFile = await _fileService.UploadAsync(model.FormFile);
            Header header = new Header()
            {

                Title = model.Title,
                Description = model.Description,
                FilePath = newFile,
            };

            await _context.AddAsync(header);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Update(int id)
        {
            var header = await _context.Headers.FirstOrDefaultAsync(h=>h.Id==id);

            if (header == null) return NotFound();

            UpdateHeaderVM updateHeaderVM = new UpdateHeaderVM()
            {
                Id=header.Id,
                Title=header.Title,
                Description=header.Description,
                FilePath = header.FilePath,
                FormFile=header.FormFile,
            };

            return View(updateHeaderVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update( UpdateHeaderVM model)
        {

            if (!ModelState.IsValid) return View(model);
           

            var header = await _context.Headers.FirstOrDefaultAsync(h => h.Id == model.Id); ;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "\\wwwroot\\assets\\img\\", header.FilePath);
            _fileService.Delete(path);
            if(model.FormFile!= null)
            {
                if (!_fileService.IsImage(model.FormFile))
                    ModelState.AddModelError("model.FormFile", "Zehmet olmasa şəkil əlavə edin");

                if (!_fileService.CheckSize(model.FormFile, 60))
                    ModelState.AddModelError("model.FormFile", "Zehmet olmasa şəkil əlavə edin");


                var newFile = await _fileService.UploadAsync(model.FormFile);
                header.FilePath = newFile;
            }
            
            header.Title = model.Title;
            header.Description = model.Description;
           
            await _context.SaveChangesAsync();

            
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var header = await _context.Headers.FindAsync(id);

            if (header == null) return NotFound();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "\\wwwroot\\assets\\img\\", header.FilePath);
            _fileService.Delete(path);
            _context.Remove(header);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
