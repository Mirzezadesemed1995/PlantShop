using PlantShop.Models;

namespace PlantShop.Areas.Admin.ViewModels.ProductVM
{
    public class ProductCategoryCreateVM
    {
        public CreateProductVM? CreateProductVM { get; set; }
        public List<Category>? Category { get; set; }

        public List<Files>? Files { get; set; }
    }
}
