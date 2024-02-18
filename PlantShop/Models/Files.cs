using System.ComponentModel.DataAnnotations.Schema;

namespace PlantShop.Models
{
    public class Files
    {
        public int Id { get; set; } 

        public string? FilePath { get; set; }

       

        public int ProductId { get; set; }  
        public Product? Product { get; set; }
        [NotMapped]
        public IFormFile? FormFile { get; set; } 
    }
}
