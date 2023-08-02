using Microsoft.AspNetCore.Http;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class CollegeLogoVm
    {
        public IFormFile ImagePath { get; set; }
        public int CollegeId { get; set; }
        [Required(ErrorMessage = "College Name is required.")]
        public string? CollegeName { get; set; }
        public string? Logo { get; set; }
        [Required(ErrorMessage = "Address1 is required.")]
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public string? City { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public int? StateId { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public int? CountryId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string? CollegeCode { get; set; }
        public string? ContectPerson { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number.")]
        public string? ContectPhone { get; set; }

        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        public string? ContectEmail { get; set; }
        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }

        // public IFormFile file { get; set; }
        public College ToCollegeDbModel()
        {
            return new College
            {
                CollegeName = CollegeName,
                Logo = Logo,
                Address1 = Address2,
                Address2 = Address2,
                City = City,
                StateId = StateId,
                CountryId = CountryId,
                CollegeCode = CollegeCode,
                ContectPerson = ContectPerson,
                ContectPhone = ContectPhone,
                ContectEmail = ContectEmail,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = CreatedBy,
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = ModifiedBy,
                ModifiedDate = CollegeId > 0 ? DateTime.UtcNow : null,
            };
        }
    }
}
