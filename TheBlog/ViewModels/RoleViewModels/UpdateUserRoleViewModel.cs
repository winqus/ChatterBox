using Microsoft.Build.Framework;
using System.ComponentModel;

namespace TheBlog.ViewModels.RoleViewModels
{
    public class UpdateUserRoleViewModel
    {
        [DisplayName("User Id")]
        [Required]
        public string UserId { get; set; }

        [DisplayName("Role Name")]
        [Required]
        public string RoleName { get; set; }

        [DisplayName("New Role Toggle status")]
        [Required]
        public bool RoleStatus { get; set; }
    }
}
