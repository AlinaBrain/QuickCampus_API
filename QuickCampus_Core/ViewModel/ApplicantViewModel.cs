using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_Core.Common;
using QuickCampus_DAL.Context;
using System.ComponentModel.DataAnnotations;

namespace QuickCampus_Core.ViewModel
{
    public class ApplicantViewModel
    {
        public static explicit operator ApplicantViewModel(TblApplicant x)
        {
            return new ApplicantViewModel
            {
                ApplicantID = x.ApplicantId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                EmailAddress = x.EmailAddress,
                PhoneNumber = x.PhoneNumber,
                HighestQualificationPercentage = x.HigestQualificationPercentage,
                MatricPercentage = x.MatricPercentage,
                IntermediatePercentage = x.IntermediatePercentage,
                PassingYear = x.PassingYear,
                StatusId = x.StatusId ?? 0,
                Comment = x.Comment,
                CollegeName = x.CollegeName,
                ClientId = x.ClientId,
                CollegeId = x.CollegeId,
                IsActive = x.IsActive,
                HighestQualification=x.HighestQualification,
            };
        }
        public int ApplicantID { get; set; }
        [Required(ErrorMessage = "FirstName is required"), MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed in FirstName.")]
       
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "FirstName is required"), MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed in LastName.")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "You must provide an email address.")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Incorrect Email")]
        [MaxLength(100, ErrorMessage = "can't exceed more than 100 characters.")]
        [EmailAddress(ErrorMessage = "Not a valid email address.")]
        public string? EmailAddress { get; set; }
        [MaxLength(25, ErrorMessage = "can't exceed more than 25 characters.")]
        [Required(ErrorMessage = "You must provide a Contact Number")]
        [Display(Name = "Contact Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Contact Number ")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "You must provide a HighestQualification")]
        public int? HighestQualification { get; set; }
        public string ? HighestQualificationName { get; set; }
        [Display(Name = "Highest Qualification  Percentage %")]
        [Required(ErrorMessage = "You must provide %.")]
        [Range(1, 100, ErrorMessage = "% should be in 1 - 100 range.")]
        public double? HighestQualificationPercentage { get; set; } = null;
        [Display(Name = "Matric  Percentage  %")]
        [Required(ErrorMessage = "You must provide %.")]
        [Range(1, 100, ErrorMessage = "% should be in 1 - 100 range.")]
        public double? MatricPercentage { get; set; } = null;
        [Display(Name = "Intermediate Percentage  %")]
        [Required(ErrorMessage = "You must provide %.")]
        [Range(1, 100, ErrorMessage = "% should be in 1 - 100 range.")]
        public double? IntermediatePercentage { get; set; }
       
        [Required(ErrorMessage = "You must provide a StatusId")]
        public int StatusId { get; set; }
        public string? Comment { get; set; } = "";
        //public DateTime? CreatedDate { get; set; }
        public DateTime? RegisteredDate { get; set; }
        public string ? CollegeName { get; set; }
        public int? ClientId { get; set; }
        //public bool? IsDeleted { get; set; }
        [Required(ErrorMessage = "You must provide a AssignedToCompany")]
        public int? AssignedToCompany { get; set; }
        [Display(Name ="ActiveStatus")]
        public bool? IsActive { get; set; }
        //public DateTime? ModifiedDate { get; set; }
        public int? CollegeId { get; set; }
        public string? PassingYear { get; set; }
        public List<SkillVmm> skilltype { get; set; }

        public TblApplicant ToApplicantDbModel()
        {
            return new TblApplicant
            {
                FirstName = FirstName,
                LastName = LastName,
                EmailAddress = EmailAddress,
                PhoneNumber = PhoneNumber,
                HigestQualificationPercentage = HighestQualificationPercentage,
                MatricPercentage = MatricPercentage,
                IntermediatePercentage = IntermediatePercentage,
                StatusId = StatusId,
                Comment = Comment,
                HighestQualification=HighestQualification,
                ClientId = ClientId,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = ApplicantID > 0 ? DateTime.UtcNow : null,
                IsActive = true,
                IsDeleted = false,
                CollegeId = CollegeId,
                PassingYear=PassingYear,
            };
        }

        public TblApplicant ToUpdateDbModel()
        {
            return new TblApplicant
            {
                ApplicantId = ApplicantID,
                FirstName = FirstName,
                LastName = LastName,
                EmailAddress = EmailAddress,
                PhoneNumber = PhoneNumber,
                HigestQualificationPercentage = HighestQualificationPercentage,
                MatricPercentage = MatricPercentage,
                IntermediatePercentage = IntermediatePercentage,
                StatusId = StatusId,
                Comment = Comment,
                ClientId = ClientId,
                ModifiedDate = DateTime.UtcNow,
                CreatedDate = ApplicantID > 0 ? DateTime.UtcNow : null,
                IsActive = true,
                IsDeleted = false,
                CollegeId = CollegeId,
                PassingYear = PassingYear,
            };
        }
        public class ApplicantGridViewModel
        {
            public int ApplicantID { get; set; }

            [Required(ErrorMessage = "Name is required"), MaxLength(20)]
            [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed.")]
            public string? FirstName { get; set; }

            [Required(ErrorMessage = "Name is required"), MaxLength(20)]
            [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed.")]
            public string? LastName { get; set; }
            public int? CollegeId { get; set; }
            public List<SelectListItem>? Colleges { get; set; }

            [Display(Name = "Email Address")]
            [Required(ErrorMessage = "You must provide an email address.")]
            [MaxLength(100, ErrorMessage = "can't exceed more than 100 characters.")]
            [EmailAddress(ErrorMessage = "Not a valid email address.")]
            public string? EmailAddress { get; set; }

            [MaxLength(25, ErrorMessage = "can't exceed more than 25 characters.")]
            [Required(ErrorMessage = "You must provide a Contact Number")]
            [Display(Name = "Contact Number")]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Contact number.")]
            public string? PhoneNumber { get; set; }
            [Display(Name = "Highest Qualification")]
            [Required(ErrorMessage = "You must provide your highest qualification.")]
            [MaxLength(100, ErrorMessage = "can't exceed more than 100 characters.")]
            public string? HighestQualification { get; set; }
            [Display(Name = "Highest Qualification %")]
            [Required(ErrorMessage = "You must provide %.")]
            [Range(1, 100, ErrorMessage = "% should be in 1 - 100 range.")]
            public double? HighestQualificationPercentage { get; set; }

            [Display(Name = "10th Class %")]
            [Required(ErrorMessage = "You must provide %.")]
            [Range(1, 100, ErrorMessage = "% should be in 1 - 100 range.")]
            public double? MatricPercentage { get; set; }

            [Display(Name = "12th Class %")]
            [Required(ErrorMessage = "You must provide %.")]
            [Range(1, 100, ErrorMessage = "% should be in 1 - 100 range.")]
            public double? IntermediatePercentage { get; set; }
            public int StatusID { get; set; }
            public int CompanyId { get; set; }
            public string? Company { get; set; }
            public string? Comment { get; set; }
            public DateTime? CreatedDate { get; set; }
            public string? CollegeName { get; set; }
            public string RegisteredDate { get { return CreatedDate.HasValue ? CreatedDate.Value.ToShortDateString() : ""; } set { } }
        }
        public class ApplicantDetails
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? EmailAddress { get; set; }
            public string? PhoneNumber { get; set; }
            public string? HighestQualification { get; set; }
            public string? HighestQualificationPercentage { get; set; }
            public string? MatricPercentage { get; set; }
            public string? IntermediatePercentage { get; set; }
           
        }

        public class ApplicantFilter
        {
            public string? Name { get; set; }

        }
        public class ApplicantValidator : AbstractValidator<ApplicantViewModel>
        {
            public ApplicantValidator()
            {

                RuleFor(x => x.HighestQualificationPercentage)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotEmpty().WithMessage("HighestQualificationPercentage could not be empty");

                RuleFor(x => x.IntermediatePercentage)
                 .Cascade(CascadeMode.StopOnFirstFailure)
                 .NotNull().WithMessage("IntermediatePercentage could not be null")
                 .NotEmpty().WithMessage("IntermediatePercentage could not be empty");


                RuleFor(x => x.MatricPercentage)
                 .Cascade(CascadeMode.StopOnFirstFailure)
                 .NotNull().WithMessage("MatricPercentage could not be null")
                 .NotEmpty().WithMessage("MatricPercentage could not be empty");

                RuleFor(x => x.EmailAddress)
                  .Cascade(CascadeMode.StopOnFirstFailure).EmailAddress().NotNull().WithMessage(" Email Address could not be null")
                  .NotEmpty().WithMessage(" Email address could not be empty")
                  .Length(5, 50).WithMessage(" Email address length could not be greater than 50");

                RuleFor(x => x.PhoneNumber)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull().WithMessage("Contact Phone could not be null")
               .NotEmpty().WithMessage("Contact Phone could not be empty")
               .Length(10, 10).WithMessage("Contact  length could not be greater than 10");
            }
        }
    }
}
