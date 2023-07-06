using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class LoginResponseVM
    {
        public string? Token { get; set; }
        public int? CilentId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleMaster> RoleMasters { get; set; }
    }

    public class RoleMaster
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public List<RolePermissions> rolePermissions { get; set; }

    }
    public class RolePermissions
    {
        public int Id { get; set; }
        public string PermissionName { get; set; }
        public string DisplayName { get; set; }
    }
}
