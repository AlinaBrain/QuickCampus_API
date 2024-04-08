using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class AddCollegeVm
    {
        public int? CollegeId { get; set; }
        [Required(ErrorMessage = "College Name is required.")]
        public string? CollegeName { get; set; }
        
        public IFormFile? ImagePath { get; set; }
        [Required(ErrorMessage = "Address1 is required.")]
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public int? CityId { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public int? StateId { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public int? CountryId { get; set; }
        [Required(ErrorMessage = "TblCollege Code is required.")]
        public string? CollegeCode { get; set; }
        [Required(ErrorMessage ="Contact Person Name is required ")]
        public string? ContactPerson { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number.")]
        public string? ContactPhone { get; set; }
        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        public string? ContactEmail { get; set; }

    }
}
