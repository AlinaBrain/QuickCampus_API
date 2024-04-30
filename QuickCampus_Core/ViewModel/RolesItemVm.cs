using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class RolesItemVm
    {
        public int ItemId { get; set; }
        public string ItemIcon { get; set; }
        public string ItemName { get; set; }
        public List<PermissionVM> ItemSubMenu { get; set; }
    }
}
