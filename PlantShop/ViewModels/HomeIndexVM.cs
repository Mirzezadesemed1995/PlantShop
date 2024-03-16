using PlantShop.Models;
using PlantShop.Models.Identity;

namespace PlantShop.ViewModels
{
    public class HomeIndexVM
    {
        public List<Product> Products { get; set; } 
        public List<Header> Headers { get; set; }   
        public User  User { get; set; }
        public List<Basket> Baskets { get; set; }
        public List<UserBasket> UserBasket { get; set; }
    }
}
