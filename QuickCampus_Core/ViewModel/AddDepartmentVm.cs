using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class AddDepartmentVm
    {
        public static explicit operator AddDepartmentVm(TblDepartment item)
        {
            return new AddDepartmentVm
            {
                Id = item.Id,
                Name = item.Name,
                IsActive= item.IsActive,
                CreatedDate = item.CreatedDate,
                ClientId = item.ClientId,
            };
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ClientId { get; set; }
    }
}
