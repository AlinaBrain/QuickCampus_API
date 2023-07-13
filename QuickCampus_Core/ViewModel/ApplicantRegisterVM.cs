using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace QuickCampus_Core.ViewModel
{
    public class ApplicantRegisterVM
    {

        public int ApplicantId { get; set; }

        [Required(ErrorMessage = "Your must provide First Name.")]
        [Display(Name = "First Name")]
        [MaxLength(100, ErrorMessage = "can't exceed more than 100 characters.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(100, ErrorMessage = "can't exceed more than 100 characters.")]
        public string LastName { get; set; }

        [Display(Name = "College Name")]
        [Required(ErrorMessage = "You must select one college.")]
        public int? CollegeId { get; set; }
        public List<SelectListItem> Colleges { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "You must provide an email address.")]
        [MaxLength(100, ErrorMessage = "can't exceed more than 100 characters.")]
        [EmailAddress(ErrorMessage = "Not a valid email address.")]
        public string EmailAddress { get; set; }

        [MaxLength(25, ErrorMessage = "can't exceed more than 25 characters.")]
        [Required(ErrorMessage = "You must provide a mobile Number")]
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid mobile number.")]
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

