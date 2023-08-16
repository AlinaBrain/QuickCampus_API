using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    internal class DepartmentVM
    {
        public int Id { get; set; }

        public string? DepartmentName { get; set; }

        public string? Description { get; set; }

    }
    internal class DepartmentResponseVM
    {
        public int Id { get; set; }

        public string? DepartmentName { get; set; }

        public string? Description { get; set; }
        public bool? IsActive { get; set; }

    }
}
