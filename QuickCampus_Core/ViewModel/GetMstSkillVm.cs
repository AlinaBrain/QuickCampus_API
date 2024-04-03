using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class GetMstSkillVm
    {
        public int SkillId { get; set; }
        [Required]
        public string? SkillName { get; set; }
        public bool? IsActive { get; set; }
        
        public int? ClientId { get; set; }
    }
}
