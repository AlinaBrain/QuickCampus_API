using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class StateVM
    {
        public int StateId { get; set; }

        public string? StateName { get; set; }

        public int? CountryId { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public virtual ICollection<College> Colleges { get; set; } = new List<College>();

        public virtual Country? Country { get; set; }

        public virtual ICollection<WalkIn> WalkIns { get; set; } = new List<WalkIn>();
    }
}
