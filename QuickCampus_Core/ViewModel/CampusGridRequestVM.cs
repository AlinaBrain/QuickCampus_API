using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class CampusGridRequestVM
    {

        [Required(ErrorMessage = "Date is required.")]
        public DateTime? WalkInDate { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string JobDescription { get; set; } = String.Empty;
        [Required(ErrorMessage = "Address1 is required"), MaxLength(60)]
        public string Address1 { get; set; }
        public string Address2 { get; set; } 
        public int WalkInID { get; set; }
        [Required]
        public int City { get; set; } 
        [Required]
        public int? StateID { get; set; }
        [Required]
        public int? CountryID { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = String.Empty;

        public string? PassingYear { get; set; }

        public List<CampusWalkInModel>? Colleges { get; set; }
    }
}
