using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantShop.Areas.Admin.ViewModels.HeaderInfoVM
{
    public class CreateHeaderInfoVM
    {
      
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
