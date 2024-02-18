using System.ComponentModel.DataAnnotations;

namespace PlantShop.Areas.Admin.ViewModels.AccountVM
{
    public class LoginVM
    {
       

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

       

        public string? ReturnUrl { get; set; }
    }
}
