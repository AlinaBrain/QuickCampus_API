using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class ActiveRoleVm
    {
        public static explicit operator ActiveRoleVm(TblRole items)
        {
            return new ActiveRoleVm
            {
                Id = items.Id,
                Name = items.Name,
                CreatedBy = items.CreatedBy,
                CreatedDate = items.CreatedDate,
                ModifiedBy = items.ModifiedBy,
                ClientId = items.ClientId,
                IsActive = items.IsActive,
                IsDeleted = items.IsDeleted,
                
                
            };
        }
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? CreatedBy { get; set; }
   
        public int? ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModofiedDate { get; set; }

        public int? ClientId { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; }
    }
}
