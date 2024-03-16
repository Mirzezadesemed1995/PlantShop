using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PlantShop.Models.Identity
{
    public class User : IdentityUser
    {
        

        

       

        public ICollection<Basket> Baskets { get; set; }
    }
}
