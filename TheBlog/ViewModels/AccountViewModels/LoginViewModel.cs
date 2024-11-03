using System.ComponentModel.DataAnnotations;
using TheBlog.Attributes.Validation;

namespace TheBlog.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [UserPassword]
        public string Password { get; set; }
    }
}
