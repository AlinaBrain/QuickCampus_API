using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class GetCampusViewModel
    {
        public static explicit operator GetCampusViewModel(TblWalkIn item)
        {
            return new GetCampusViewModel
            {
                WalkInDate = item.WalkInDate,
                WalkInID=item.WalkInId,
                Address1 = item.Address1,
                Address2 = item.Address2,
                City = item.City,
                StateID=item.StateId,
                CountryID=item.CountryId,
                Title=item.Title,
                IsActive=item.IsActive,
                JobDescription=item.JobDescription,
                PassingYear=item.PassingYear,
                ClientId=item.ClientId,
            };
        }
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
        public bool? IsActive { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        public string? Title { get; set; }
        public string? PassingYear { get; set; }
        public int? ClientId { get; set; }
        public List<CampusWalkInModel> Colleges { get; set; }
    }
}
