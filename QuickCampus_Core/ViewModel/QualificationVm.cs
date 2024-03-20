using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class QualificationVm 
    {
        public static explicit operator QualificationVm(TblQualification items)
        {
            return new QualificationVm
            {
                Id = items.Id,
                QualificationName = items.QualificationName,
               
            };
        }
        public int Id { get; set; }

        public string? QualificationName { get; set; }

        public bool? IsAcive { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
