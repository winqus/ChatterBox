using System.ComponentModel.DataAnnotations;
using TheBlog.Attributes.Validation;

namespace TheBlog.Models.AccountViewModels
{
    public class PasswordResetViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [UserPassword]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Repeat Password")]
        [Compare("Password", ErrorMessage = "Both passwords have to match.")]
        [UserPassword]
        public string RepeatPassword { get; set; }

        public string Code { get; set; }

        public string UserId { get; set; }
    }
}
