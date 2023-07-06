using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class RoleModel
    {
        public int userId { get; set;}
        [Required(ErrorMessage = "RoleName is required.")]
        [MaxLength(20, ErrorMessage = "Name must be at most 20 characters long.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]*$", ErrorMessage = "Only alphabetic characters are allowed in the name.")]
        public string? RoleName { get; set; }
    }
}
