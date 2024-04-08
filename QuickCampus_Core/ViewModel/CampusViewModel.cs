using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_DAL.Context;
using System.ComponentModel.DataAnnotations;


namespace QuickCampus_Core.ViewModel
{
    public class CampusViewModel
    {
        public static explicit operator CampusViewModel(TblWalkIn x)
        {
            return new CampusViewModel
            {
                WalkInID = x.WalkInId,
                Address1 = x.Address1,
                Address2 = x.Address2,
                City = x.City,
                IsActive = x.IsActive,
                WalkInDate = x.WalkInDate,
                JobDescription = x.JobDescription,
                Title = x.Title,
                StateId = x.StateId,
                CountryId = x.CountryId,
                PassingYear= x.PassingYear,
                ClientId=x.ClientId,
                
            };
        }

        public List<CampusWalkInModel> CampusList { get; set; }
        public int WalkInID { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public int? City { get; set; }

  
        public bool? IsActive { get; set; }
        public DateTime? WalkInDate { get; set; }
        public string? JobDescription { get; set; }
        public string? Title { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public string? PassingYear { get; set; }
        public int? ClientId { get; set; }
    }
    public class CampusGridViewModel
    {
        
        public int WalkInID { get; set; }
        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public int? City { get; set; }

        public int? StateID { get; set; }

        public int? CountryID { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime? WalkInDate { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string? JobDescription { get; set; }

        //[Required( ErrorMessage = "Time is required." )]
        //[DisplayFormat( DataFormatString = "hh:mm tt" )]
        //public string WalkInStartTime { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        public string? Title { get; set; }
        public string? PassingYear { get; set; }
        public int? ClientId { get; set; }
        public List<CampusWalkInModel> Colleges { get; set; }

    }

    public class CampusWalkInModel
    {
        public int CampusId { get; set; }
        [Required]
        public int? CollegeId { get; set; }
        [Required(ErrorMessage = "Time is required.")]
        public string? ExamStartTime { get; set; }

        [Required(ErrorMessage = "Time is required.")]

        public string? ExamEndTime { get; set; }

        [Required(ErrorMessage = "Time is required.")]
        public bool IsIncludeInWalkIn { get; set; }
        public DateTime? StartDateTime { get; set; }
        public string? CollegeName { get; set; }
        public string? CollegeCode { get; set; }

    }
    public class CollegeFilter
    {
        public string? CollegeName { get; set; }
    }
    public class CountryModel
    {
        public int CountryID { get; set; }
        public string? CountryName { get; set; }
    }
    public class StateModel
    {
        public int StateID { get; set; }
        public string? StateName { get; set; }
        public int CountryID { get; set; }
    }
}
