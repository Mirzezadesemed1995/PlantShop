using System.ComponentModel.DataAnnotations.Schema;

namespace PlantShop.Areas.Admin.ViewModels.CategoryVM
{
    public class UpdateCategoryVM
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? FilePath { get; set; }

        [NotMapped]
        public IFormFile? formFile { get; set; }
    }
}
