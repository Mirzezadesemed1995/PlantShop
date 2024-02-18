using Microsoft.AspNetCore.Mvc.Rendering;
using PlantShop.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantShop.Areas.Admin.ViewModels.ProductVM
{
    public class CreateProductVM
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public double OldPrice { get; set; }
        public double NewPrice { get; set; }
        public string? FilePath { get; set; }

        public string? Description { get; set; }
        public Discount Discount { get; set; }
        public int CategoryId { get; set; }
        public ICollection<SelectListItem>? Categories { get; set; }
        public IFormFile? MainPhoto { get; set; }
        public ICollection<IFormFile>? Files { get; set; }
    }
}
