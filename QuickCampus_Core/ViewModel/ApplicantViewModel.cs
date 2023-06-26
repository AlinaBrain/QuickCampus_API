using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class ApplicantViewModel
    {
        public static explicit operator ApplicantViewModel(Applicant x)
        {
            return new ApplicantViewModel
            {
                ApplicantID = x.ApplicantId,
                ApplicantToken = x.ApplicantToken,
                FirstName = x.FirstName,
                LastName = x.LastName,
                EmailAddress = x.EmailAddress,
                PhoneNumber = x.PhoneNumber,
                HigestQualification = x.HigestQualification,
                HigestQualificationPercentage = x.HigestQualificationPercentage,
                MatricPercentage = x.MatricPercentage,
                IntermediatePercentage = x.IntermediatePercentage,
                Skills = x.Skills,
                StatusID = x.StatusId ?? 0,
                Comment = x.Comment,
                RegisteredDate = x.RegisteredDate,
                CollegeName = x.CollegeName
            };
        }

        public ApplicantFilter filter { get; set; }
        //public bool IsActive { get; set; }
        //public bool IsDeleted { get; set; }
        public IEnumerable<ApplicantGridViewModel> ApplicantList { get; set; }
        
        public int ApplicantID { get; private set; }
        public string? ApplicantToken { get; private set; }
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? EmailAddress { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string? HigestQualification { get; private set; }
        public decimal? HigestQualificationPercentage { get; private set; }
        public decimal? MatricPercentage { get; private set; }
        public decimal? IntermediatePercentage { get; private set; }
        public string? Skills { get; private set; }
        public int StatusID { get; private set; }
        public string? Comment { get; private set; }
        public DateTime? CreatedDate  { get; private set; }
        public DateTime? RegisteredDate { get; private set; }
        public string? CollegeName { get; set; }

        //public Applicant ToApplicantDbModel()
        //{
        //    return new Applicant
        //    {
               
        //        ApplicantToken = ApplicantToken,
        //        FirstName = FirstName,
        //        LastName = LastName,
        //        EmailAddress = EmailAddress,
        //        PhoneNumber = PhoneNumber,
        //        HigestQualification = HigestQualification,
        //        HigestQualificationPercentage = HigestQualificationPercentage,
        //        MatricPercentage = MatricPercentage,
        //        IntermediatePercentage = IntermediatePercentage,
        //        Skills = Skills,
        //        StatusID = StatusID,
        //        Comment = Comment,
        //        CollegeName = CollegeName
        //        //CreatedDate = RegisteredDate,
        //    };
        //}
        //public Applicant ToUpdateDbModel()
        //{
        //    return new Applicant
        //    {
        //        ApplicantId = ApplicantID,
        //        ApplicantToken = ApplicantToken,
        //        FirstName = FirstName,
        //        LastName = LastName,
        //        EmailAddress = EmailAddress,
        //        PhoneNumber = PhoneNumber,
        //        HigestQualification = HigestQualification,
        //        HigestQualificationPercentage = HigestQualificationPercentage,
        //        MatricPercentage = MatricPercentage,
        //        IntermediatePercentage = IntermediatePercentage,
        //        Skills = Skills,
        //        StatusID = StatusID,
        //        Comment = Comment,
        //        RegisteredDate = RegisteredDate,
        //        CollegeName = CollegeName
        //    };
        //}



        public class ApplicantGridViewModel
        {
            public int ApplicantID { get; set; }
            public string ApplicantToken { get; set; }
            [Required(ErrorMessage = "Your must provide First Name.")]
            [Display(Name = "First Name")]
            [MaxLength(100, ErrorMessage = "can't exceed more than 100 characters.")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            [MaxLength(100, ErrorMessage = "can't exceed more than 100 characters.")]
            public string LastName { get; set; }

            [Display(Name = "College Name")]
            [Required(ErrorMessage = "You must select one college.")]
            public int? CollegeId { get; set; }
            public List<SelectListItem> Colleges { get; set; }

            [Display(Name = "Email Address")]
            [Required(ErrorMessage = "You must provide an email address.")]
            [MaxLength(100, ErrorMessage = "can't exceed more than 100 characters.")]
            [EmailAddress(ErrorMessage = "Not a valid email address.")]
            public string EmailAddress { get; set; }

            //[MaxLength(25, ErrorMessage = "can't exceed more than 25 characters.")]
            //[Required(ErrorMessage = "You must provide a Contact Number")]
            //[Display(Name = "Contact Number")]
            //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Contact number.")]
            public string PhoneNumber { get; set; }



            [Display(Name = "Higest Qualification")]
            [Required(ErrorMessage = "You must provide your highest qualification.")]
            [MaxLength(100, ErrorMessage = "can't exceed more than 100 characters.")]
            public string HigestQualification { get; set; }

            [Display(Name = "Best 3 Skills")]
            [MaxLength(100, ErrorMessage = "can't exceed more than 100 characters.")]
            public string Skills { get; set; }

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
            public string Company { get; set; }
            public string Comment { get; set; }
            public DateTime? CreatedDate { get; set; }
            public string? CollegeName { get; set; }
            public string RegisteredDate { get { return CreatedDate.HasValue ? CreatedDate.Value.ToShortDateString() : ""; } set { } }
            //public CollegeGridViewModel College { get; set; }
        }

        public class ApplicantDetails
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string EmailAddress { get; set; }
            public string PhoneNumber { get; set; }
            public string HigestQualification { get; set; }
            public string HigestQualificationPercentage { get; set; }
            public string MatricPercentage { get; set; }
            public string IntermediatePercentage { get; set; }
            public string Skills { get; set; }
        }

        public class ApplicantFilter
        {
            public string Name { get; set; }

        }
    }
}
