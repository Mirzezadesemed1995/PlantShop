using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.Areas.Admin.ViewModels.HeaderInfoVM;
using PlantShop.DAL;
using PlantShop.Models;

namespace PlantShop.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class HeaderInfoController : Controller
    {
        readonly private AppDbContext _context;

        public HeaderInfoController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<HeaderInfo> headerInfos = await _context.HeaderInfos.ToListAsync();
            return View(headerInfos);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var headerInfos = await _context.HeaderInfos.FindAsync(id);
            if (headerInfos == null) return NotFound();
            return View(headerInfos);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        => View();
        [HttpPost]
        public async Task<IActionResult> Create(CreateHeaderInfoVM model)
        {
            if (ModelState.IsValid) return View();
            HeaderInfo headerInfo = new()
            {
                Title = model.Title,
                Description = model.Description,

            };

            await _context.HeaderInfos.AddAsync(headerInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Update(int id)
        {
            var header = await _context.HeaderInfos.FindAsync(id);

            if (header == null) return NotFound();

            UpdateHeaderInfo headerInfo = new UpdateHeaderInfo()
            {
                Id = header.Id,
                Title = header.Title,
                Description = header.Description,

            };

            return View(headerInfo);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateHeaderInfo model)
        {
            var headerinfo = await _context.HeaderInfos.FindAsync(model.Id);
            if (headerinfo == null) return NotFound();
            headerinfo.Title = model.Title;
            headerinfo.Description = model.Description;

             _context.HeaderInfos.Update(headerinfo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var headerInfo = await _context.HeaderInfos.FindAsync(id);

            if (headerInfo == null) return NotFound();

            _context.HeaderInfos.Remove(headerInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
