using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class CollegeCountryStateVmmm
    {
        public static explicit operator CollegeCountryStateVmmm(TblCollege items)
        {
            return new CollegeCountryStateVmmm
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
                ContactPersonName = items.ContactPersonName,
                ContactPhone = items.ContectPhone,
                ContactEmail = items.ContectEmail,
                ClientId = items.ClientId,
                IsActive = items.IsActive,
            };
        }

        public int CollegeId { get; set; }
        [Required(ErrorMessage = "TblCollege Name is required.")]
        public string? CollegeName { get; set; }
        public string? Logo { get; set; }
        [Required(ErrorMessage = "Address1 is required.")]
        public string? Address1 { get; set; }
        [Required(ErrorMessage = "Address2 is required.")]
        public string? Address2 { get; set; }
        [Required(ErrorMessage = "MstCity is required.")]
        public int? CityId { get; set; }
        [Required(ErrorMessage = "StateId is required.")]
        public int? StateId { get; set; }
        [Required(ErrorMessage = "MstCity_State_Country is required.")]
        public int? CountryId { get; set; }
        public bool? IsActive { get; set; }

        //public int? CreatedBy { get; set; }
        [Required(ErrorMessage = "CollegeCode is required.")]
        public string? CollegeCode { get; set; }
        [EmailAddress(ErrorMessage = "Enter a valid contact person email.")]
        public string? ContactPersonName { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number.")]
        public string? ContactPhone { get; set; }

        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        public string? ContactEmail { get; set; }
        public string ClientName { get; set; }
        public int? ClientId { get; set; }
        

    }

    
}

