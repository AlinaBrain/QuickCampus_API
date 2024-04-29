using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class RolesItemVm
    {
        public int ItmeId { get; set; }
        public string ItmeIcon { get; set; }
        public string ItmeName { get; set; }
        public List<PermissionVM> ItemSubMenu { get; set; }
    }
}
