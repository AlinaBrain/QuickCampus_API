using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class RoleViewVm
    {
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public bool? IsActive { get; set; }
        public List<RolePermissions> RolesPermission { get; set; }

        public static explicit operator RoleViewVm(TblRole v)
        {
            return new RoleViewVm
            {
                RoleId = v.Id,
                RoleName = v.Name,
                IsActive = v.IsActive
            };
        }
    }
}
