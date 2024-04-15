using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class TemplateVm
    {
        public static explicit operator TemplateVm(TblTemplate item)
        {
            return new TemplateVm
            {
                Id = item.Id,
                Subject = item.Subject,
                Body = item.Body,
                CreatedAt = item.CreatedAt,
                CreatedBy = item.CreatedBy,
                ModifiedAt = item.ModifiedAt,
                ModifiedBy= item.ModifiedBy,
                ClientId = item.ClientId,
                IsActive=item.IsActive,
                IsDeleted=item.IsDeleted,
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
        public TblTemplate ToTemplateDbModel()
        {
            return new TblTemplate
            {
                Subject = Subject,
                Body = Body,
                CreatedBy = CreatedBy,
                ModifiedAt = Id > 0 ? DateTime.UtcNow : null,
                IsActive = true,
                IsDeleted = false,
                ClientId= ClientId,
                CreatedAt=DateTime.Now,
                ModifiedBy = ModifiedBy
            };
        }
    }
}
