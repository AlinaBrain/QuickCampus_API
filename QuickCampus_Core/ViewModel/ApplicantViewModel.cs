using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_Core.Common;
using QuickCampus_DAL.Context;
using System.ComponentModel.DataAnnotations;

namespace QuickCampus_Core.ViewModel
{
    public class ApplicantViewModel
    {
        public static explicit operator ApplicantViewModel(Applicant x)
        {
            return new ApplicantViewModel
            {
                ApplicantID = x.ApplicantId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                EmailAddress = x.EmailAddress,
                PhoneNumber = x.PhoneNumber,
                HigestQualification = x.HigestQualification,
                HigestQualificationPercentage = x.HigestQualificationPercentage,
                MatricPercentage = x.MatricPercentage,
                IntermediatePercentage = x.IntermediatePercentage,
                Skills = x.Skills,
                StatusId = x.StatusId ?? 0,
                Comment = x.Comment,
                AssignedToCompany = x.AssignedToCompany,
                CollegeName = x.CollegeName,
                ClientId = x.ClientId,
                CollegeId = x.CollegeId,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted,
            };
        }
        public int ApplicantID { get; set; }

        
        public string? FirstName { get; set; }
        
        public string? LastName { get; set; }

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
        [Required(ErrorMessage = "You must provide a HighestQualification")]
        public string? HigestQualification { get; set; }
        [Display(Name = "Higest Qualification  parcentage %")]
        [Required(ErrorMessage = "You must provide %.")]
        [Range(1, 100, ErrorMessage = "% should be in 1 - 100 range.")]
        public decimal? HigestQualificationPercentage { get; set; } = null;
        [Display(Name = "Matric  Parcentage  %")]
        [Required(ErrorMessage = "You must provide %.")]
        [Range(1, 100, ErrorMessage = "% should be in 1 - 100 range.")]
        public decimal? MatricPercentage { get; set; } =null;
        [Display(Name = "Intermediate Parcentage  %")]
        [Required(ErrorMessage = "You must provide %.")]
        [Range(1, 100, ErrorMessage = "% should be in 1 - 100 range.")]
        public decimal? IntermediatePercentage { get; set; }
        [Required(ErrorMessage = "You must provide a Skills ")]
        public string? Skills { get; set; }
        [Required(ErrorMessage = "You must provide a StatusId")]
        public int StatusId { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? RegisteredDate { get; set; }
        [Required(ErrorMessage ="College Name is Must Required")]
        public string CollegeName { get; set; }
        public int? ClientId { get; set; }

        public bool? IsDeleted { get; set; }
        [Required(ErrorMessage = "You must provide a AssignedToCompany")]
        public int? AssignedToCompany { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CollegeId { get; set; }
  
        public Applicant ToApplicantDbModel()
        {
            return new Applicant
            {
                FirstName = FirstName,
                LastName = LastName,
                EmailAddress = EmailAddress,
                PhoneNumber = PhoneNumber,
                HigestQualification = HigestQualification,
                HigestQualificationPercentage = HigestQualificationPercentage,
                MatricPercentage = MatricPercentage,
                IntermediatePercentage = IntermediatePercentage,
                Skills = Skills,
                StatusId = StatusId ,
                Comment = Comment,
                AssignedToCompany = AssignedToCompany,
                CollegeName = CollegeName,
                ClientId = ClientId,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = ApplicantID > 0 ? DateTime.UtcNow : null,
                IsActive = true,
                IsDeleted = false,
                CollegeId   = CollegeId,
            };
        }

        public Applicant ToUpdateDbModel()
        {
            return new Applicant
            {
                ApplicantId = ApplicantID,
                FirstName = FirstName,
                LastName = LastName,
                EmailAddress = EmailAddress,
                PhoneNumber = PhoneNumber,
                HigestQualification = HigestQualification,
                HigestQualificationPercentage = HigestQualificationPercentage,
                MatricPercentage = MatricPercentage,
                IntermediatePercentage = IntermediatePercentage,
                Skills = Skills,
                StatusId = StatusId ,
                Comment = Comment,
               AssignedToCompany= AssignedToCompany,
                CollegeName = CollegeName,
                ClientId = ClientId,
                ModifiedDate = DateTime.UtcNow,
                CreatedDate = ApplicantID > 0 ? DateTime.UtcNow : null,
                IsActive = true,
                IsDeleted = false,
                CollegeId = CollegeId,
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
            public string PhoneNumber { get; set; }
            [Display(Name = "Higest Qualification")]
            [Required(ErrorMessage = "You must provide your highest qualification.")]
            [MaxLength(100, ErrorMessage = "can't exceed more than 100 characters.")]
            public string? HigestQualification { get; set; }

            [Display(Name = "Best 3 Skills")]
            [MaxLength(100, ErrorMessage = "can't exceed more than 100 characters.")]
            public string? Skills { get; set; }

            [Display(Name = "Higest Qualification %")]
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
            public string? HigestQualification { get; set; }
            public string? HigestQualificationPercentage { get; set; }
            public string? MatricPercentage { get; set; }
            public string? IntermediatePercentage { get; set; }
            public string? Skills { get; set; }
        }

        public class ApplicantFilter
        {
            public string? Name { get; set; }

        }
        public class ApplicantValidator : AbstractValidator<ApplicantViewModel>
        {
            public ApplicantValidator()
            {
                RuleFor(x => x.CollegeName)
                     .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("College Name could not be null")
                  .NotEmpty().WithMessage("College Name could not be empty")
            .Matches(@"^[A-Za-z\s]*$").WithMessage("'{PropertyName}' should only contain letters.")
            .Length(3, 250);
                RuleFor(x => x.Skills)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("Skills could not be null")
                  .NotEmpty().WithMessage("Skills could not be empty")
                  .Length(3, 200).WithMessage("Skills length 3 to 200 Characters");

                RuleFor(x => x.Comment)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("Comments could not be null")
                  .NotEmpty().WithMessage("Comments could not be empty");

                RuleFor(x => x.FirstName)
                 .Cascade(CascadeMode.StopOnFirstFailure)
                 .NotNull().WithMessage("FirstName could not be null")
                 .NotEmpty().WithMessage("FirstName could not be empty")
                  .Length(4, 50).WithMessage("FirstName length could 4 to 50 character");

                RuleFor(x => x.LastName)
                 .Cascade(CascadeMode.StopOnFirstFailure)
                 .NotNull().WithMessage("LastName could not be null")
                 .NotEmpty().WithMessage("LastName could not be empty")
                  .Length(3, 50).WithMessage("FirstName length could 3 to 50 character");

                RuleFor(x => x.HigestQualificationPercentage)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                 
                  .NotEmpty().WithMessage("HigestQualificationPercentage could not be empty");


                RuleFor(x => x.HigestQualification)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("HigestQualification could not be null")
                  .NotEmpty().WithMessage("HigestQualification could not be empty");


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
               .NotNull().WithMessage("Contect Phone could not be null")
               .NotEmpty().WithMessage("Contect Phone could not be empty")
               .Length(10, 10).WithMessage("Contact  length could not be greater than 10");
            }
        }
    }
}
