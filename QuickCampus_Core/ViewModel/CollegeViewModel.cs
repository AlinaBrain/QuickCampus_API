using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class CollegeViewModel
    {
        public CollegeFilter filter { get; set; }
        public IEnumerable<CollegeGridViewModel> CollegeList { get; set; }
    }
    public class CollegeGridViewModel
    {
        public int CollegeID { get; set; }
        [Required(ErrorMessage = "College Name is required.")]
        public string CollegeName { get; set; }
        public string LogoImage { get; set; }
        public string LogoImagePath { get; set; }
        [Required(ErrorMessage = "Address1 is required.")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public int? StateID { get; set; }
        public string StateName { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public string? CountryName { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public int? CountryID { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CollegeCode { get; set; }
        public string ContectPerson { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number.")]
        public string ContectPhone { get; set; }

        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        public string ContectEmail { get; set; }


    }
    //public class CollegeFilter
    //{
    //    public string CollegeName { get; set; }
    //}
    //public class CountryModel
    //{
    //    public int CountryID { get; set; }
    //    public string CountryName { get; set; }
    //}
    //public class StateModel
    //{
    //    public int StateID { get; set; }
    //    public string StateName { get; set; }
    //    public int CountryID { get; set; }
    //}

}
