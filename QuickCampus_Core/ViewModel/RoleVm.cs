using QuickCampus_DAL.Context;
using System.ComponentModel.DataAnnotations;

namespace QuickCampus_Core.ViewModel
{
    public class RoleVm
    {
        public static explicit operator RoleVm(TblRole item)
        {
            return new RoleVm
            {  
                ClientId = item.ClientId,
                Name = item.Name,
            };

        }
        public int? ClientId { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "RoleName is required"), MaxLength(30)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed.")]
        public string? Name { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModofiedDate { get; set; }
        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; }

        public TblRole ToRoleDBModel()
        {
            return new TblRole
            {
                Id = Id,
                ClientId= ClientId,
                Name = Name,
                CreatedBy = CreatedBy,
                ModifiedBy = ModifiedBy,
                CreatedDate = DateTime.Now,
                ModofiedDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false
            };
        }
    }

    public class RoleResponse
    {
        public static explicit operator RoleResponse(TblRole item)
        {
            return new RoleResponse
            {
                Id = item.Id,
                ClientId = item.ClientId,
                Name = item.Name,
                CreatedBy = item.CreatedBy,
                ModifiedBy = item.ModifiedBy,
                CreatedDate = item.CreatedDate,
                ModofiedDate = item.ModofiedDate,
                IsDeleted = item.IsDeleted,
                IsActive = item.IsActive
            };

        }
        public int? ClientId { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModofiedDate { get; set; }
        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; }

    }
}
