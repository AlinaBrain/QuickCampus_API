using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class MstSkillsVm
    {
        public static explicit operator MstSkillsVm(MstSkill item)
        {
            return new MstSkillsVm
            {
                SkillId = item.SkillId,
                SkillName = item.SkillName,
                IsActive = item.IsActive,
                ClientId= item.ClientId,
            };
        }

        public int SkillId { get; set; }
        [Required]
        public string? SkillName { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDelete { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public int? ClientId { get; set; }

        public MstSkill ToMstSkillDbMOdel()
        {
            return new MstSkill
            {
                SkillName=SkillName,
                IsActive=true,
                IsDelete=false,
                ClientId=ClientId,
                CreatedDate=DateTime.UtcNow,
                ModifiedDate= SkillId >0 ? DateTime.UtcNow : null
            };
        }


    }
}
