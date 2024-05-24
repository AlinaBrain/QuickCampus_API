using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class DepartmentVm
    {
        public static explicit operator DepartmentVm( TblDepartment item)
        {
            return new DepartmentVm
            {
                Id= item.Id,
                Name = item.Name,
                IsDeleted=item.IsDeleted,
                IsActive=item.IsActive,
                ModifiedDate = item.ModifiedDate,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                ModifiedBy=item.ModifiedBy,
                ClientId=item.ClientId,
            };
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ClientId { get; set; }
    }
}
