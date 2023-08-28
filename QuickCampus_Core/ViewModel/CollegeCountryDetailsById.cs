using QuickCampus_DAL.Context;
using System.ComponentModel.DataAnnotations;

namespace QuickCampus_Core.ViewModel
{
    public class CollegeCountryDetailsById
    {
        public static explicit operator CollegeCountryDetailsById(College items)
        {
            return new CollegeCountryDetailsById
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
                ModifiedBy = items.ModifiedBy,
                ClientId = items.ClientId,
                IsActive = items.IsActive,
                IsDeleted = items.IsDeleted,
                CreatedBy = items.CreatedBy
            };
        }
        public int CollegeId { get; set; }
        [Required(ErrorMessage = "College Name is required.")]
        public string? CollegeName { get; set; }
        public string? Logo { get; set; }
        [Required(ErrorMessage = "Address1 is required.")]
        public string? Address1 { get; set; }
        [Required(ErrorMessage = "Address2 is required.")]
        public string? Address2 { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public int? CityId { get; set; }
        [Required(ErrorMessage = "StateId is required.")]
        public int? StateId { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public int? CountryId { get; set; }
        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }
        [Required(ErrorMessage = "CollegeCode is required.")]
        public string? CollegeCode { get; set; }
        [EmailAddress(ErrorMessage = "Enter a valid contact person email.")]
        public string? ContectPerson { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number.")]
        public string? ContectPhone { get; set; }

        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        public string? ContectEmail { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public int? ClientId { get; set; }

    }
}
