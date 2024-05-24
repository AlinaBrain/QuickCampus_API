using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class CampusGridRequestVM
    {

        [Required(ErrorMessage = "Date is required.")]
        public DateTime? WalkInDate { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string JobDescription { get; set; } = String.Empty;
        [Required(ErrorMessage = "Address1 is required"), MaxLength(60)]
        public string Address1 { get; set; }
        public string Address2 { get; set; } 
        public int WalkInID { get; set; }
        [Required]
        public int City { get; set; } 
        [Required]
        public int? StateId { get; set; }
        [Required]
        public int? CountryId { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = String.Empty;
        [Required]
        public string? PassingYear { get; set; }
        [Required]
        public int? ClientId { get; set; }

        public List<CampusWalkInModel>? Colleges { get; set; }

        public TblWalkIn ToCampusAddDbModel()
        {
            return new TblWalkIn
            {
                WalkInDate = WalkInDate,
                JobDescription = JobDescription,
                Address1=Address1,
                Address2=Address2,
                City=City,
                ClientId=ClientId,  
                CountryId=CountryId,
                StateId=StateId,
                Title=Title, 
                PassingYear=PassingYear,
            };
        }
        public TblWalkIn ToCampusUpdateDbModel()
        {
            return new TblWalkIn
            {
                WalkInId = WalkInID,
                WalkInDate = WalkInDate,
                JobDescription = JobDescription,
                Address1 = Address1,
                Address2 = Address2,
                City = City,
                ClientId = ClientId,
                CountryId = CountryId,
                StateId = StateId,
                Title = Title,
                PassingYear = PassingYear,
            };
        }
    }
}
