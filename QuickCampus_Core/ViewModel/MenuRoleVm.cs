using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class MenuRoleVm
    {
        public int UserId { get; set; }
        public List<SubMenuRoleVm> MenuItem { get; set; }
    }
  
}
