using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShop.Areas.Admin.ViewModels.CategoryVM;
using PlantShop.Areas.Admin.ViewModels.ProductVM;
using PlantShop.DAL;
using PlantShop.Helpers;
using PlantShop.Models;
using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames;

namespace PlantShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {

        readonly private AppDbContext _context;
        readonly private IFileService _fileService;
        public ProductController(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {

            List<Product> products = await _context.Product
                                                   .Include(p => p.Categories)


                                                   .Select(pr => new Product()
                                                   {
                                                       Id = pr.Id,
                                                       Name = pr.Name,
                                                       Description = pr.Description,
                                                       Discount = pr.Discount,
                                                       CategoryId = pr.CategoryId,
                                                       Categories = pr.Categories,
                                                       FilePath = pr.FilePath,
                                                       Files = pr.Files,
                                                   })
                                                   .OrderByDescending(pr => pr.Id)
                                                   .ToListAsync();


            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            List<Category> category = await _context.Category.ToListAsync();

            var product = new CreateProductVM();
            ProductCategoryCreateVM vm = new ProductCategoryCreateVM()
            {
                Category = category,
                CreateProductVM = product
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCategoryCreateVM? model)
        {
			List<Category> category = await _context.Category.ToListAsync();
            model.Category = category;
			if (!ModelState.IsValid) return View(model);


            
				if (!_fileService.IsImage(model.CreateProductVM.MainPhoto))
				{
					ModelState.AddModelError("model.CreateProductVM.Files", "Please upload an image");
					return View(model);
				}

				int maxSize = 250;
				if (!_fileService.CheckSize(model.CreateProductVM.MainPhoto, maxSize))
				{
					ModelState.AddModelError("model.CreateProductVM.Files", $"Please upload an image smaller than {maxSize} KB");
					return View(model);
				}
			

			

            Product product = new Product()
            {
                Name = model.CreateProductVM.Name,
                CategoryId = model.CreateProductVM.CategoryId,
                NewPrice = model.CreateProductVM.NewPrice,
                OldPrice = model.CreateProductVM.OldPrice,
                Description = model.CreateProductVM.Description,
                Discount = model.CreateProductVM.Discount,
                FilePath = await _fileService.UploadAsync(model.CreateProductVM.MainPhoto),
            };

            await _context.Product.AddAsync(product);
            await _context.SaveChangesAsync();


            if (model.CreateProductVM.Files != null)
            {
				foreach (var image in model.CreateProductVM.Files)
				{
					if (!_fileService.IsImage(image))
						ModelState.AddModelError("model.CreateProductVM.Files", "Please upload an image");

					if (!_fileService.CheckSize(image, maxSize))
						ModelState.AddModelError("model.CreateProductVM.Files", $"Please upload an image smaller than {maxSize} KB");

					var filename = await _fileService.UploadAsync(image);
					var uploadFile = new Files
					{
						ProductId = product.Id,
						FilePath = filename,

					};

					await _context.Files.AddAsync(uploadFile);
				}
				await _context.SaveChangesAsync();

			}





			await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null) return NotFound();
            UpdateProductVM updateProductVM = new UpdateProductVM()
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Discount = product.Discount,
                NewPrice = product.NewPrice,
                OldPrice = product.OldPrice,
                FilePath = product.FilePath,
            };


            var model = new ProductCategoryUpdateVM
            {
                UpdateProductVM = updateProductVM,
                Category = await _context.Category.ToListAsync()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductCategoryUpdateVM model)
        {
            var product = await _context.Product.FindAsync(model.UpdateProductVM.Id);
            if (product == null) return BadRequest();
            int maxSize = 60;

            if (model.UpdateProductVM.MainPhoto != null)
            {
                if (!_fileService.IsImage(model.UpdateProductVM.MainPhoto))
                {
                    ModelState.AddModelError("model.CreateProductVM.Files", "Please upload an image");
                    return View(model);
                }

                if (!_fileService.CheckSize(model.UpdateProductVM.MainPhoto, maxSize))
                {
                    ModelState.AddModelError("model.CreateProductVM.Files", $"Please upload an image smaller than {maxSize} KB");
                    return View(model);
                }

                product.FilePath = await _fileService.UploadAsync(model.UpdateProductVM.MainPhoto);
            }


            if (model.UpdateProductVM.Files != null)
            {
                foreach (var image in model.UpdateProductVM.Files)
                {
                    if (!_fileService.IsImage(image))
                    {
                        ModelState.AddModelError("model.CreateProductVM.Files", "Please upload an image");
                        return View(model);
                    }

                    if (!_fileService.CheckSize(image, maxSize))
                    {
                        ModelState.AddModelError("model.CreateProductVM.Files", $"Please upload an image smaller than {maxSize} KB");
                        return View(model);
                    }
                    var filename = await _fileService.UploadAsync(image);
                    var newFile = new Files
                    {
                        ProductId = product.Id,
                        FilePath = filename,

                    };

                    await _context.Files.AddAsync(newFile);
                }
                await _context.SaveChangesAsync();

            }

            product.Name = model.UpdateProductVM.Name;
            product.Discount = model.UpdateProductVM.Discount;
            product.NewPrice = model.UpdateProductVM.NewPrice;
            product.OldPrice = model.UpdateProductVM.OldPrice;
            product.Description = model.UpdateProductVM.Description;
            product.CategoryId = model.UpdateProductVM.CategoryId;
            _context.Product.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Product
             .FindAsync(id);
            if (product == null) return NotFound();

            var files=await _context.Files.Where(f=>f.ProductId==product.Id).ToListAsync();

            foreach (var file in files)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "\\wwwroot\\assets\\img\\", file.FilePath);

                _fileService.Delete(path);
            }
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
