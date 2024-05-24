using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class RoleMappingRequest
    {
        [Required]
        public int RoleId { get; set; }
        public List<Permission>? permissions { get; set; }
    }

    public class Permission
    {
        [Required]
        public  int PermissionIds { get; set; }
    }
}
