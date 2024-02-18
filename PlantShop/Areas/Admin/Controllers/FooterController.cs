using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.Areas.Admin.ViewModels.FooterVM;
using PlantShop.Areas.Admin.ViewModels.GoogleMapVM;
using PlantShop.DAL;
using PlantShop.Helpers;
using PlantShop.Models;

namespace PlantShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FooterController : Controller
    {

        readonly private AppDbContext _context;
        readonly private IFileService _fileService;
        public FooterController(AppDbContext context, IFileService fileService)
        {

            _context = context;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            List<Footer> footers = await _context.Footers.ToListAsync();
            return View(footers);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var footers = await _context.Footers.FindAsync(id);
            return View(footers);
        }
        public async Task<IActionResult> Create()
        {

            return View();


        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateFooterVM model)
        {
            if (!ModelState.IsValid) return View(model);



            Footer footer = new Footer()
            {

                Title = model.Title,
                Description = model.Description,
                Adress = model.Adress,
                Email = model.Email,
                Phone = model.Phone,
            };

            await _context.AddAsync(footer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var footer = await _context.Footers.FirstOrDefaultAsync(h => h.Id == id);

            if (footer == null) return NotFound();

            UpdateFooterVM updateFooterVM = new UpdateFooterVM()
            {
                Id = footer.Id,
                Title = footer.Title,
                Description = footer.Description,
                Adress = footer.Adress,
                Email = footer.Email,
                Phone = footer.Phone

            };

            return View(updateFooterVM);
        }
        [HttpPost]

        public async Task<IActionResult> Update(UpdateFooterVM model)
        {

            if (!ModelState.IsValid) return View(model);


            var footer = await _context.Footers.FirstOrDefaultAsync(h => h.Id == model.Id); ;


            footer.Title = model.Title;
            footer.Description = model.Description;
            footer.Adress = model.Adress;
            footer.Email = model.Email;
            footer.Phone = model.Phone;

            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var footer = await _context.Footers.FindAsync(id);

            if (footer == null) return NotFound();

            _context.Remove(footer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
