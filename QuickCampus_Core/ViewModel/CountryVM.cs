using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class CountryVM
    {
        public int CountryId { get; set; }

        public string? CountryName { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public virtual ICollection<College> Colleges { get; set; } = new List<College>();

        public virtual ICollection<State> States { get; set; } = new List<State>();

        public virtual ICollection<WalkIn> WalkIns { get; set; } = new List<WalkIn>();
    }
}
