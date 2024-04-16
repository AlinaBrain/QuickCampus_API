using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class AddSubjectVm
    {
        public static explicit operator AddSubjectVm(TblSubject item)
        {
            return new AddSubjectVm
            {
                Id= item.Id,
                Name= item.Name,
                DepartmentId= item.DepartmentId,
                CreatedDate= item.CreatedDate,
                ClientId= item.ClientId,
            };
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ClientId { get; set; }
    }
}
