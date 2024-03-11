using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PlantShop.Models.Identity
{
    public class User : IdentityUser
    {
        

        

        // Password property is not needed as ASP.NET Identity handles password internally

       

        public ICollection<Basket> Baskets { get; set; }
    }
}
