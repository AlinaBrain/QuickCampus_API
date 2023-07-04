using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class ApplicantRegisterViewModel
    {
        public int ApplicantId { get; set; }

        [Required(ErrorMessage = "First Name is required"), MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required"), MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed.")]
        public string LastName { get; set; }

        
        public int? CollegeId { get; set; }
        public List<SelectListItem> Colleges { get; set; }

        [Required, MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
      
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        [RegularExpression(@"^(0|91)?[1-9][0-9]{9}$", ErrorMessage = "Invalid Mobile Number.")]

        public string PhoneNumber { get; set; }



        [Display(Name = "Highest Qualification")]
        [Required(ErrorMessage = "You must provide your highest qualification.")]
        [MaxLength(100, ErrorMessage = "can't exceed more than 100 characters.")]
        public string HigestQualification { get; set; }

        [Display(Name = "Best 3 Skills")]
        [MaxLength(100, ErrorMessage = "can't exceed more than 100 characters.")]
        public string Skills { get; set; }

        [Display(Name = "Highest Qualification %")]
        [Required(ErrorMessage = "You must provide %.")]
        [Range(1, 100, ErrorMessage = "% should be in 1 - 100 range.")]
        public decimal? HigestQualificationPercentage { get; set; }

        [Display(Name = "10th Class %")]
        [Required(ErrorMessage = "You must provide %.")]
        [Range(1, 100, ErrorMessage = "% should be in 1 - 100 range.")]
        public decimal? MatricPercentage { get; set; }

        [Display(Name = "12th Class %")]
        [Required(ErrorMessage = "You must provide %.")]
        [Range(1, 100, ErrorMessage = "% should be in 1 - 100 range.")]
        public decimal? IntermediatePercentage { get; set; }

        public string ApplicantCollegeLogo { get; set; }

        public string ApplicantCollegeName { get; set; }

        //public WalkInViewModel WalkIn { get; set; }
    
}
}
