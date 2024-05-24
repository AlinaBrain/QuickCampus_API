using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class DashboardVm 
    {
        
        public string ?Icon { get; set; }
        public string ? Url { get; set; }
        public List<DashVm> DashData { get; set; }
    }
}
