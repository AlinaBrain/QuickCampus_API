using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class StateModelVm
    {
        public int? StateId { get; set; }
        [Required(ErrorMessage = "State Name is required.")]
        [MaxLength(50, ErrorMessage = "Name must be at most 20 characters long.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]*$", ErrorMessage = "Only alphabetic characters are allowed in the name.")]
        public string? StateName { get; set; }

        public int? CountryId { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }
        public int? ClientId { get; set; }
    }
}
