using System.ComponentModel.DataAnnotations;

namespace TheBlog.Models.AccountViewModels
{
    public class RestorePasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
