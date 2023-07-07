using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class UserRoleMapping
    {
        public int userId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public int? ClientId { get; set; }
        public List<UserRoleVm>? userRoleVms { get; set; }
    }
    public class UserRoleVm
    {
        public int Id  { get; set; }
        public string? RoleName { get; set; }

        public List<RolePermissionVm>? rolePermissions { get; set; }
    }
    public class RolePermissionVm
    {
        public int  Id { get; set; }
        public string? PermissionName { get; set; }
    }
}
