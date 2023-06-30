using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class RoleVm
    {
        public static explicit operator RoleVm(TblRole item)
        {
            return new RoleVm
            {
                Id = item.Id,
                Name = item.Name,
                CreatedBy = item.CreatedBy,
                ModifiedBy = item.ModifiedBy,
                CreatedDate = item.CreatedDate,
                ModofiedDate =item.ModofiedDate,

            };

        }
        public int Id { get; set; }
        [Required(ErrorMessage = "RoleName is required"), MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed.")]
        public string? Name { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModofiedDate { get; set; }


        public TblRole toRoleDBModel()
        {
            return new TblRole
            {
                Id = Id,
                Name = Name,
                CreatedBy = CreatedBy,
                ModifiedBy = ModifiedBy,
                CreatedDate = DateTime.Now,
                ModofiedDate = DateTime.Now,
            };
        }
    }
}
