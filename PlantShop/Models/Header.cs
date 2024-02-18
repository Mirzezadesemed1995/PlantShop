using System.ComponentModel.DataAnnotations.Schema;

namespace PlantShop.Models
{
    public class Header
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public string?  FilePath { get; set; }
        [NotMapped]
        public IFormFile? FormFile { get; set; }
    }
}
