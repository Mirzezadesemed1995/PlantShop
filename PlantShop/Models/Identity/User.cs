using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PlantShop.Models.Identity
{
    public class User : IdentityUser
    {
        [Required]
        public string FullName { get; set; }

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        // Password property is not needed as ASP.NET Identity handles password internally

        [Required(ErrorMessage = "The Confirm Password field is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }

        public ICollection<Basket> Baskets { get; set; }
    }
}
