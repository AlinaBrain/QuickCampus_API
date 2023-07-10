using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class RoleMappingRequest
    {
        public List<RoleIds>? roleIds { get; set; }
    }

    public class RoleIds
    {
        public int RoleId { get; set; }
        public List<int>? PermissionIds { get; set; }
    }
}
