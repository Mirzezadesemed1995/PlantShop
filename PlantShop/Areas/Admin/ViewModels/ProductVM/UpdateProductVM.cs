using PlantShop.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantShop.Areas.Admin.ViewModels.ProductVM
{
    public class UpdateProductVM
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public double OldPrice { get; set; }
        public double NewPrice { get; set; }
        public string? FilePath { get; set; }

        public string? Description { get; set; }
        public Discount Discount { get; set; }
        public int CategoryId { get; set; }
        public ICollection<Category> Categories { get; set; }
        [NotMapped]
        public IFormFile? MainPhoto { get; set; }
        public ICollection<IFormFile>? Files { get; set; }
    }
}
