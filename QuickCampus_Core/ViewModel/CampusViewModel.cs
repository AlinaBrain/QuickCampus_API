using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_DAL.Context;
using System.ComponentModel.DataAnnotations;


namespace QuickCampus_Core.ViewModel
{
    public class CampusViewModel
    {
        public static explicit operator CampusViewModel(WalkIn x)
        {
            return new CampusViewModel
            {
                WalkInID = x.WalkInId,
                Address1 = x.Address1,
                Address2 = x.Address2,
                City = x.City,
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate,
                WalkInDate = x.WalkInDate,
                JobDescription = x.JobDescription,
                Title = x.Title,
                Colleges = x.CampusWalkInColleges,
                StateId=x.StateId,
                CountryId=x.CountryId,
               
                
            };
        }

        public IEnumerable<CampusGridViewModel> CampusList { get; set; }
        public int WalkInID { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? StateName { get;  set; }
        public string? CountryName { get; set; }
        public DateTime? CreatedDate { get;  set; }
        public bool? IsActive { get; set; }
        public DateTime? WalkInDate { get; set; }
        public string? JobDescription { get; set; }
        public string? Title { get; set; }
        public object Colleges { get; set; }
        public object StateId { get; set; }
        
        public int? CountryId { get; set; }

       

    }
    public class CampusGridViewModel
    {
        public int WalkInID { get; set; }


        //[Required( ErrorMessage = "Address is required." )]
        public string? Address1 { get; set; }

        public string? Address2 { get; set; }
        //[Required( ErrorMessage = "City is required." )]      
        public string? City { get; set; }

        //[Required( ErrorMessage = "State is required." )]
        public int? StateID { get; set; }

        public string? StateName { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }

        public string? CountryName { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        //[Required( ErrorMessage = "Country is required." )]
        public int? CountryID { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? WalkInDate { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string? JobDescription { get; set; }

        //[Required( ErrorMessage = "Time is required." )]
        //[DisplayFormat( DataFormatString = "hh:mm tt" )]
        //public string WalkInStartTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? CreatedDate { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        public string? Title { get; set; }
        public int CreatedBy { get; set; }
        public List<CampusWalkInModel> Colleges { get; set; }

    }

    public class CampusWalkInModel
    {
        public int CampusId { get; set; }
        [Required]
        public int? CollegeId { get; set; }

        [Required]
        public int StateId { get; set; }
        [Required]
        public string? CollegeName { get; set; }

        [Required(ErrorMessage = "Time is required.")]
        
        public string? ExamStartTime { get; set; }

        [Required(ErrorMessage = "Time is required.")]
       
        public string? ExamEndTime { get; set; }

        [Required(ErrorMessage = "Time is required.")]
        public bool IsIncludeInWalkIn { get; set; }
        [Required]
        public string? CollegeCode { get; set; }

        public DateTime? StartDateTime { get; set; }

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
