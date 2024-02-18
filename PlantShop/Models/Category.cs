using System.ComponentModel.DataAnnotations.Schema;

namespace PlantShop.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? FilePath { get; set; }

        [NotMapped]
        public IFormFile? formFile { get; set; }

        public List<Product> Product { get; set; }
    }
}
