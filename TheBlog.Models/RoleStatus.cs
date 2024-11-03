using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBlog.Models
{
    public class RoleStatus
    {
        [DisplayName("RoleName")]
        public string RoleName { get; init; }

        [DisplayName("Assigned")]
        public bool Assigned { get; init; }
    }
}
