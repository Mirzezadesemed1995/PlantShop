using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.Areas.Admin.ViewModels.CommentVM;
using PlantShop.Areas.Admin.ViewModels.PlantVM;
using PlantShop.Areas.Admin.ViewModels.ShoppingVM;
using PlantShop.DAL;
using PlantShop.Helpers;
using PlantShop.Models;

namespace PlantShop.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CommentController : Controller
    {

        readonly private AppDbContext _context;
        readonly private IFileService _fileService;

        public CommentController(AppDbContext context, IFileService fileService)
        {

            _context = context;
            _fileService = fileService;
        }
        public async Task <IActionResult> Index()
        {
            List<Comment> comments =await _context.Comments.ToListAsync();
            return View(comments);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var comments = await _context.Comments.FindAsync(id);
            return View(comments);
        }
        public async Task<IActionResult> Create()
        {

            return View();


        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentVM model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!_fileService.IsImage(model.FormFile))
                ModelState.AddModelError("model.FormFile", "Zehmet olmasa şəkil əlavə edin");
            int maxSize = 60;
            if (!_fileService.CheckSize(model.FormFile, maxSize))
                ModelState.AddModelError("model.FormFile", $"Zehmet olmasa {maxSize} kicik olculu  şəkil əlavə edin");


            var newFile = await _fileService.UploadAsync(model.FormFile);
            Comment comment = new Comment()
            {

                Title = model.Title,
                Description = model.Description,
                FilePath = newFile,
            };

            await _context.AddAsync(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var comment = await _context.Stoppings.FirstOrDefaultAsync(h => h.Id == id);

            if (comment == null) return NotFound();

            UpdatePlantVM updateCommentVM = new UpdatePlantVM()
            {
                Id = comment.Id,
                Title = comment.Title,
                Description = comment.Description,
                FilePath = comment.FilePath,
                FormFile = comment.FormFile,
            };

            return View(updateCommentVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateCommentVM model)
        {

            if (!ModelState.IsValid) return View(model);


            var comment = await _context.Comments.FirstOrDefaultAsync(h => h.Id == model.Id); ;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "\\wwwroot\\assets\\img\\", comment.FilePath);
            _fileService.Delete(path);
            if (model.FormFile != null)
            {
                if (!_fileService.IsImage(model.FormFile))
                    ModelState.AddModelError("model.FormFile", "Zehmet olmasa şəkil əlavə edin");

                if (!_fileService.CheckSize(model.FormFile, 60))
                    ModelState.AddModelError("model.FormFile", "Zehmet olmasa şəkil əlavə edin");


                var newFile = await _fileService.UploadAsync(model.FormFile);
                comment.FilePath = newFile;
            }

            comment.Title = model.Title;
            comment.Description = model.Description;

            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null) return NotFound();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "\\wwwroot\\assets\\img\\", comment.FilePath);
            _fileService.Delete(path);
            _context.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
