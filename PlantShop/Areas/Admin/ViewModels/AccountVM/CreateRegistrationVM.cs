using System.ComponentModel.DataAnnotations;

namespace PlantShop.Areas.Admin.ViewModels.AccountVM
{
    public class CreateRegistrationVM
    {
        [Required]
        public string FullName { get; set; }

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [Required(ErrorMessage = "The Confirm Password field is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
