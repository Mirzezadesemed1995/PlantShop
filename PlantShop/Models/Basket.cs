using PlantShop.Models.Identity;

namespace PlantShop.Models
{
    public class Basket
    {
        public int Id { get; set; } 
        public string UserId { get; set; }

        public User? User { get; set; }
       
        public ICollection<UserBasket>? UserBaskets { get; set; }
    }
}
