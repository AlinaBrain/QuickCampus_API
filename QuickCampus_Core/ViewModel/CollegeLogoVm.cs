using Microsoft.AspNetCore.Http;
using QuickCampus_DAL.Context;
using System.ComponentModel.DataAnnotations;

namespace QuickCampus_Core.ViewModel
{
    public class CollegeLogoVm
    {
        public static explicit operator CollegeLogoVm(College items)
        {
            return new CollegeLogoVm
            {
                CollegeId = items.CollegeId,
                CollegeName = items.CollegeName,
                Address1 = items.Address2,
                Logo = items.Logo,
                Address2 = items.Address2,
                CityId = items.CityId,
                StateId = items.StateId,
                CountryId = items.CountryId,
                CollegeCode = items.CollegeCode,
                ContectPerson = items.ContectPerson,
                ContectPhone = items.ContectPhone,
                ContectEmail = items.ContectEmail,
                ClientId = items.ClientId,
                ModifiedDate = items.ModifiedDate,
               CreatedBy = items.CreatedBy,
               ModifiedBy = items.ModifiedBy,
               CreatedDate = items.CreatedDate
            };
        }

        public IFormFile? ImagePath { get; set; }=null;
        public int CollegeId { get; set; }
        [Required(ErrorMessage = "College Name is required.")]
        public string? CollegeName { get; set; }
        public string? Logo { get; set; }
        [Required(ErrorMessage = "Address1 is required.")]
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public int? CityId { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public int? StateId { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public int? CountryId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public string? CollegeCode { get; set; }
        public string? ContectPerson { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number.")]
        public string? ContectPhone { get; set; }

        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        public string? ContectEmail { get; set; }
        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; } 
        public bool? IsDeleted { get; set; }
        public int? ClientId { get; set; }

    }
}
