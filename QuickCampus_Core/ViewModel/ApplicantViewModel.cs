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
            };
        }

       // public ApplicantFilter filter { get; set; }
        //public bool IsActive { get; set; }
        //public bool IsDeleted { get; set; }
       // public IEnumerable<ApplicantGridViewModel> ApplicantList { get; set; }
        
        public int ApplicantID { get;  set; }
        
        [Required(ErrorMessage = "Name is required"), MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed.")]
        public string? FirstName { get;  set; }
        [Required(ErrorMessage = "Name is required"), MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed.")]
        public string? LastName { get;  set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "You must provide an email address.")]
        [MaxLength(100, ErrorMessage = "can't exceed more than 100 characters.")]
        [EmailAddress(ErrorMessage = "Not a valid email address.")]
        public string? EmailAddress { get;  set; }
        [MaxLength(25, ErrorMessage = "can't exceed more than 25 characters.")]
        [Required(ErrorMessage = "You must provide a Contact Number")]
        [Display(Name = "Contact Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Contact number.")]
        public string? PhoneNumber { get;  set; }
        [MaxLength(50, ErrorMessage = "can't exceed more than 50 characters.")]
        [Required(ErrorMessage = "You must provide a HighestQualification")]
        public string? HigestQualification { get;  set; }
        [PercentageRange(ErrorMessage = "Percentage must be between 0 and 100.")]
        public decimal? HigestQualificationPercentage { get;  set; }
        [PercentageRange(ErrorMessage = "Percentage must be between 0 and 100.")]
        public decimal? MatricPercentage { get;  set; }
        [PercentageRange(ErrorMessage = "Percentage must be between 0 and 100.")]
        public decimal? IntermediatePercentage { get;  set; }
        [Required(ErrorMessage = "You must provide a HighestQualification")]
        public string? Skills { get;  set; }
        [Required(ErrorMessage = "You must provide a StatusId")]
        public int? StatusId { get; set; }
        public string? Comment { get;  set; }
        public DateTime? CreatedDate  { get; set; }
        public DateTime? RegisteredDate { get;  set; }
        public string? CollegeName { get; set; }
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
                StatusId = StatusId ?? 0,
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
                StatusId = StatusId ?? 0,
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

            [Display(Name = "College Name")]
            [Required(ErrorMessage = "You must select one college.")]
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
    }
}
