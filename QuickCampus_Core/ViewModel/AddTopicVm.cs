using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class AddTopicVm
    {
        public static explicit operator AddTopicVm(TblTopic item)
        {
            return new AddTopicVm
            {
                Id= item.Id,
                Name= item.Name,
                DepartmentId= item.DepartmentId,
                SubjectId= item.SubjectId,
                IsActive = item.IsActive,
                CreatedDate= item.CreatedDate,
                ClientId= item.ClientId,
            };
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? DepartmentId { get; set; }
        public int? SubjectId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ClientId { get; set; }
    }
}
