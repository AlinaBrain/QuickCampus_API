using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class SubjectVm
    {
        public static explicit operator SubjectVm(TblSubject item)
        {
            return new SubjectVm
            {
                Id= item.Id,
                Name=item.Name,
                DepartmentId=item.DepartmentId,
                IsActive=item.IsActive,
                CreatedBy=item.CreatedBy,
                CreatedDate=item.CreatedDate,
                ClientId=item.ClientId,
            };
        }
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? DepartmentId { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ClientId { get; set; }
    }
}
