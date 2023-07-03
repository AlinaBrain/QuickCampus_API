
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class RolePermissionVM
    {
        public int Id { get; set; }
        public string? Rolename { get; set; }
        public List<PermissionVM>? Permissions { get; set; }
    }
}
