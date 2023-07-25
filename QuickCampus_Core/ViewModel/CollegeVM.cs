using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_DAL.Context;
using System.ComponentModel.DataAnnotations;

namespace QuickCampus_Core.ViewModel
{
    public class CollegeVM
    {
        public static explicit operator CollegeVM(College items)
        {
            return new CollegeVM
            {
                CollegeName = items.CollegeName,
                Logo = items.Logo,
                Address1 = items.Address2,
                Address2 = items.Address2,
                City = items.City,
                StateID = items.StateId,
                CountryID = items.CountryId,
                CollegeCode = items.CollegeCode,
                ContectPerson = items.ContectPerson,
                ContectPhone = items.ContectPhone,
                ContectEmail = items.ContectEmail
            };
        }
        public int CollegeID { get; set; }
        [Required(ErrorMessage = "College Name is required.")]
        public string? CollegeName { get; set; }
        public string? Logo { get; set; }
        [Required(ErrorMessage = "Address1 is required.")]
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public string? City { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public int? StateID { get; set; }
        public string? StateName { get; set; }
        public IEnumerable<SelectListItem>? States { get; set; }
        public string? CountryName { get; set; }
        public IEnumerable<SelectListItem>? Countries { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public int? CountryID { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string? CollegeCode { get; set; }
        public string? ContectPerson { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number.")]
        public string? ContectPhone { get; set; }

        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        public string? ContectEmail { get; set; }

         public College ToCollegeDbModel()
        {
            return new College
            {
                CollegeName = CollegeName,
                Logo = Logo,
                Address1 = Address2,
                Address2 = Address2,
                City = City,
                StateId = StateID,
                CountryId = CountryID,
                CollegeCode = CollegeCode,
                ContectPerson = ContectPerson,
                ContectPhone = ContectPhone,
                ContectEmail = ContectEmail,
                CreatedDate = CreatedDate,
                CreatedBy = CreatedBy,
                IsActive = true,
                IsDeleted = false,
            };
        }

    }

}
