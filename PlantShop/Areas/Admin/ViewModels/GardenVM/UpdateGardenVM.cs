using System.ComponentModel.DataAnnotations.Schema;

namespace PlantShop.Areas.Admin.ViewModels.GardenVM
{
    public class UpdateGardenVM
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? FilePath { get; set; }
        [NotMapped]
        public IFormFile? FormFile { get; set; }
    }
}
