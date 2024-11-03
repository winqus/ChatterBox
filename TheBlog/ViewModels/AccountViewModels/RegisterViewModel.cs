using System.ComponentModel.DataAnnotations;
using TheBlog.Attributes.Validation;

namespace TheBlog.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [UserName]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [UserPassword]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Repeat Password")]
        [Compare("Password", ErrorMessage = "Both passwords have to match.")]
        public string RepeatPassword { get; set; }
    }
}
