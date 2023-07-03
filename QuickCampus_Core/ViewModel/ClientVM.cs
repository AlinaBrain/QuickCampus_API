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
    public class ClientVM

    {
        public static explicit operator ClientVM(TblClient items)
        {
            return new ClientVM
            {
                Id = items.Id,
                Name = items.Name,
                CraetedBy = items.CraetedBy,
                CreatedDate = items.CreatedDate,
                ModifiedBy = items.ModifiedBy,
                ModofiedDate = items.ModofiedDate,
            };
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required"), MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed.")]

        public string? Name { get; set; }

        public int? CraetedBy { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ModofiedDate { get; set; }

       
       //ublic IEnumerable<SelectListItem> rec { get; set; }

        public TblClient ToClientDbModel()
        {
            return new TblClient
            {
                Name = Name,

                CraetedBy = CraetedBy,
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = ModifiedBy,
                ModofiedDate = DateTime.UtcNow,
            };
        }

        public TblClient ToUpdateDbModel()
        {
            return new TblClient
            {
                Id = Id,
                Name = Name,
                CraetedBy = CraetedBy,
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = ModifiedBy,
                ModofiedDate = DateTime.UtcNow,
            };
        }
    }
}
