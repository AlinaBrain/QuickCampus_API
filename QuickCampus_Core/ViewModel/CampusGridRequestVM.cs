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
        public string Address1 { get; set; }=String.Empty;
        public string Address2 { get; set; } = String.Empty;
        public int WalkInID { get; set; }
        public int City { get; set; } 
        [Required]
        public int? StateID { get; set; }
        [Required]
        public int? CountryID { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = String.Empty;

        public List<CampusWalkInModel>? Colleges { get; set; }
    }
}
