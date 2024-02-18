using PlantShop.Models;

namespace PlantShop.ViewModels
{
	public class ShopDetailsVM
	{
		public Product Product { get; set; }

		public List<Files> Files { get; set; }

		public Category Category { get; set; }
	}
}
