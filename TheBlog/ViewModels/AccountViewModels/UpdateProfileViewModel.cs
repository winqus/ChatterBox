using System.ComponentModel.DataAnnotations;
using TheBlog.Attributes.Validation;

namespace TheBlog.Models.AccountViewModels
{
    public class UpdateProfileViewModel
    {
        [EmailAddress]
        [Display(Name = "New Email")]
        public string? NewEmail { get; set; } = string.Empty;

        [UserName]
        [Display(Name = "New Username")]
        public string? NewUsername { get; set; } = string.Empty;

        [UserPassword]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; } = string.Empty;

        [UserPassword]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [Compare("NewPassword", ErrorMessage = "Both passwords have to match.")]
        public string? RepeatNewPassword { get; set; } = string.Empty;

        [Required]
        [UserPassword]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}
