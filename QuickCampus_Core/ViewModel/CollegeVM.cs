using FluentValidation;
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
                CollegeID=items.CollegeId,
                CollegeName = items.CollegeName,
                Logo = items.Logo,
                Address1 = items.Address2,
                Address2 = items.Address2,
                City = items.City,
                StateId = items.StateId,
                CountryID = items.CountryId,
                CollegeCode = items.CollegeCode,
                ContectPerson = items.ContectPerson,
                ContectPhone = items.ContectPhone,
                ContectEmail = items.ContectEmail,
                ModifiedBy = items.ModifiedBy,
                ModifiedDate = items.ModifiedDate,
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
        public int? StateId { get; set; }
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
        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

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
                CountryId = CountryID,
                CollegeCode = CollegeCode,
                ContectPerson = ContectPerson,
                ContectPhone = ContectPhone,
                ContectEmail = ContectEmail,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = CreatedBy,
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = ModifiedBy,
                ModifiedDate = CollegeID > 0 ? DateTime.UtcNow : null,
            };
        }

        public College ToUpdateDbModel()
        {
            return new College
            {
                CollegeId = CollegeID,
                CollegeName = CollegeName,
                Logo = Logo,
                Address1 = Address1,
                Address2 = Address2,
                City = City,
                StateId = StateId,
                CountryId = CountryID,
                CollegeCode = CollegeCode,
                ContectPerson = ContectPerson,
                ContectPhone = ContectPhone,
                ContectEmail = ContectEmail,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = CreatedBy,
                CreatedDate = CollegeID < 0 ? DateTime.Now : null,
                ModifiedBy = ModifiedBy,
                ModifiedDate = DateTime.UtcNow,
            };
        }

        public class CollegeValidator : AbstractValidator<CollegeVM>
        {
            public CollegeValidator()
            {
                RuleFor(x => x.CollegeName)
                     .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("College Name could not be null")

                  .NotEmpty().WithMessage("College Name could not be empty")
            .Matches(@"^[A-Za-z\s]*$").WithMessage("'{PropertyName}' should only contain letters.")
            .Length(3, 250);
                RuleFor(x => x.Address1)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("Address could not be null")
                  .NotEmpty().WithMessage("Address could not be empty")
                  .Length(5, 200).WithMessage("Address lengh could not be greater than 100");

                RuleFor(x => x.City)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("City could not be null")
                  .NotEmpty().WithMessage("City could not be empty");

                RuleFor(x => x.ContectPerson)
                  .Cascade(CascadeMode.StopOnFirstFailure).EmailAddress()
                  .NotNull().WithMessage("Contect Person could not be null")
                  .NotEmpty().WithMessage("Contect Person could not be empty");


                RuleFor(x => x.ContectEmail)
                  .Cascade(CascadeMode.StopOnFirstFailure).NotNull().WithMessage("Contect Email could not be null")
                  .NotEmpty().WithMessage("Contect Email could not be empty")
                  .Length(5, 50).WithMessage("Contect Email lengh could not be greater than 50");

                RuleFor(x => x.ContectPhone)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull().WithMessage("Contect Phone could not be null")
               .NotEmpty().WithMessage("Contect Phone could not be empty")
               .Length(10, 10).WithMessage("UserName lengh could not be greater than 100");
            }
        }

    }

}
