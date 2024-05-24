using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class TopicVm
    {
        public static explicit operator TopicVm(TblTopic item)
        {
            return new TopicVm
            {
                Id = item.Id,
                Name=item.Name,
                DepartmentId = item.DepartmentId,
                SubjectId = item.SubjectId,
                IsActive = item.IsActive,
                IsDeleted = item.IsDeleted,
                CreatedDate = item.CreatedDate,
                CreatedBy=item.CreatedBy,
                ModifiedBy=item.ModifiedBy,
                ModifiedDate=item.ModifiedDate
            };
        }
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? DepartmentId { get; set; }

        public int? SubjectId { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ClientId { get; set; }
    }
}
