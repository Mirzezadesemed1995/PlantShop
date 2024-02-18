using PlantShop.Models;

namespace PlantShop.Areas.Admin.ViewModels.ProductVM
{
    public class ProductCategoryUpdateVM
    {
        public  UpdateProductVM UpdateProductVM { get; set; }

        public ICollection<Category>? Category { get; set; }
    }
}
