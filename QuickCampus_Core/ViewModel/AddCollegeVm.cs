using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public int? collegeId { get; set; }
        [Required(ErrorMessage = "College Name is required.")]
        public string? collegeName { get; set; }
        
        public IFormFile? imagePath { get; set; }
        [Required(ErrorMessage = "Address1 is required.")]
        public string? address1 { get; set; }
        public string? address2 { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public int? cityId { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public int? stateId { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public int? countryId { get; set; }
        [Required(ErrorMessage = "TblCollege Code is required.")]
        public string? collegeCode { get; set; }
        [Required(ErrorMessage ="Contact Person Name is required ")]
        public string? contactPersonName { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number.")]
        public string? contactPhone { get; set; }
        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        public string? contactEmail { get; set; }
        public int? clientId { get; set; }

    }
}
