﻿using FluentValidation;
using Microsoft.AspNetCore.Http;
using QuickCampus_DAL.Context;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuickCampus_Core.ViewModel
{
    public class CollegeVM
    {
        public static explicit operator CollegeVM(TblCollege items)
        {
            return new CollegeVM
            {
                CollegeId=items.CollegeId,
                CollegeName = items.CollegeName,
                Address1 = items.Address2,
                Logo = items.Logo,
                Address2 = items.Address2,
               CityId=items.CityId,
                StateId = items.StateId,
                CountryId = items.CountryId,
                CollegeCode = items.CollegeCode,
                ContectPersonName = items.ContactPersonName,
                ContectPhone = items.ContectPhone,
                ContectEmail = items.ContectEmail,
                ModifiedBy = items.ModifiedBy,
                ModifiedDate = items.ModifiedDate,
                ClientId = items.ClientId,
                IsActive=items.IsActive,
                IsDeleted=items.IsDeleted,
               CreatedDate=items.CreatedDate,
               CreatedBy=items.CreatedBy
              
            };
        }
        public int CollegeId { get; set; }
        [Required(ErrorMessage = "TblCollege Name is required.")]
        public string? CollegeName { get; set; }
        public string ? Logo { get; set; }
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
        public DateTime? CreatedDate { get; set; }
        public int ?CreatedBy { get; set; }
        [Required(ErrorMessage = "CollegeCode is required.")]
        public string? CollegeCode { get; set; }
        [EmailAddress(ErrorMessage = "Enter a valid contact person email.")]
        public string? ContectPersonName { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number.")]
        public string? ContectPhone { get; set; }

        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        public string? ContectEmail { get; set; }
        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; } 
        public bool? IsDeleted { get; set; }
        public int? ClientId { get; set; }

        public TblCollege ToCollegeDbModel()
        {
            return new TblCollege
            {
                CollegeName = CollegeName,
                Logo = Logo,
                Address1 = Address2,
                Address2 = Address2,
                CityId = CityId,
                StateId = StateId,
                CountryId = CountryId,
                CollegeCode = CollegeCode,
                ContactPersonName = ContectPersonName,
                ContectPhone = ContectPhone,
                ContectEmail = ContectEmail,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = CreatedBy,
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = ModifiedBy,
                ModifiedDate = DateTime.UtcNow,
                ClientId = ClientId,

            };
        }

        public TblCollege ToUpdateDbModel()
        {
            return new TblCollege
            {
                CollegeId = CollegeId,
                CollegeName = CollegeName,
                Logo = Logo,
                Address1 = Address1,
                Address2 = Address2,
                CityId= CityId,
                StateId = StateId,
                CountryId = CountryId,
                CollegeCode = CollegeCode,
                ContactPersonName = ContectPersonName,
                ContectPhone = ContectPhone,
                ContectEmail = ContectEmail,
                CreatedBy = CreatedBy, 
                ModifiedBy = ModifiedBy,
                ModifiedDate = DateTime.Now,
                ClientId= ClientId,
                CreatedDate= CollegeId < 0 ? DateTime.UtcNow : null,
            };
        }

        public class CollegeValidator : AbstractValidator<CollegeVM>
        {
            public CollegeValidator()
            {
                RuleFor(x => x.CollegeName)
                     .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("TblCollege Name could not be null")
                  .NotEmpty().WithMessage("TblCollege Name could not be empty")
            .Matches(@"^[A-Za-z\s]*$").WithMessage("'{PropertyName}' should only contain letters.")
            .Length(3, 250);
                RuleFor(x => x.Address1)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("Address could not be null")
                  .NotEmpty().WithMessage("Address could not be empty")
                  .Length(5, 200).WithMessage("Address lengh could not be greater than 100");

                RuleFor(x => x.CityId)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("MstCity could not be null")
                  .NotEmpty().WithMessage("MstCity could not be empty");

                RuleFor(x => x.ContectPersonName)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("Contect Person could not be null")
                  .NotEmpty().WithMessage("Contect Person could not be empty");


                RuleFor(x => x.ContectEmail)
                  .Cascade(CascadeMode.StopOnFirstFailure).EmailAddress().NotNull().WithMessage("Contect Email could not be null")
                  .NotEmpty().WithMessage("Contect Email could not be empty")
                  .Length(5, 50).WithMessage("Contect Email length could not be greater than 50");

                RuleFor(x => x.ContectPhone)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull().WithMessage("Contect Phone could not be null")
               .NotEmpty().WithMessage("Contect Phone could not be empty")
               .Length(10, 10).WithMessage("Contact phone length could not be greater than 10");
            }
        }

    }

}
