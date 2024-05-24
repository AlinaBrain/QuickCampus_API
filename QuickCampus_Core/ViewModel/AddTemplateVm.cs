using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class AddTemplateVm
    {
        public static explicit operator AddTemplateVm(TblTemplate item)
        {
            return new AddTemplateVm
            {
                Subject = item.Subject,
                Body = item.Body,
                CreatedAt = item.CreatedAt,
                CreatedBy = item.CreatedBy,
                ClientId = item.ClientId, 
                IsActive=item.IsActive,
                IsDeleted=item.IsDeleted
            };
        }
        public int Id { get; set; }

        public string? Subject { get; set; }

        public string? Body { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public int? ClientId { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

    }
}
