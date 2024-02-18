using System.ComponentModel.DataAnnotations.Schema;

namespace PlantShop.Areas.Admin.ViewModels.HeaderVM
{
    public class CreateHeaderVM
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? FilePath { get; set; }
        [NotMapped] 
        public IFormFile FormFile { get; set; }
    }
}
