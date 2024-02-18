using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantShop.Areas.Admin.ViewModels.HeaderInfoVM
{
    public class UpdateHeaderInfo
    {
        public int Id { get; set; }
      
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
