using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class ClientVM
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? CraetedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime ModofiedDate { get; set; }

        public virtual TblUser? CraetedByNavigation { get; set; }

        public virtual TblUser? ModifiedByNavigation { get; set; }
    }
}
