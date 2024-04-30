using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class PermissionVM
    {
        public int Id { get; set; }
        public string? PermissionName { get; set; }
        public string? PermissionDisplay { get; set;}
        public string? DisplayIcon { get; set;}
    }
}
