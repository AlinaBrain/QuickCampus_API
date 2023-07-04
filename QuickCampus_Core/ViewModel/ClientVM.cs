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
                Address = items.Address,
                Phone= items.Phone,
                Email = items.Email,
                SubscriptionPlan = items.SubscriptionPlan,
                Geolocation = items.Geolocation,
                IsActive = items.IsActive,
                IsDeleted = items.IsDeleted,
            };
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed.")]

        public string? Name { get; set; }

        public int? CraetedBy { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ModofiedDate { get; set; }
        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Geolocation { get; set; }

        public string? SubscriptionPlan { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }


        //ublic IEnumerable<SelectListItem> rec { get; set; }

        public TblClient ToClientDbModel()
        {
            return new TblClient
            {
                Name = Name,
                Phone = Phone,
                Email = Email,
                Geolocation = Geolocation,
                SubscriptionPlan = SubscriptionPlan,
                CraetedBy = CraetedBy,
                CreatedDate = CreatedDate,
                ModifiedBy = ModifiedBy,
                ModofiedDate = ModofiedDate,
                IsActive = true,
                IsDeleted = false,


               
            };
        }

        public TblClient ToUpdateDbModel()
        {
            return new TblClient
            {
                Id = Id,
                Name = Name,
                Phone= Phone,
                Email = Email,
                SubscriptionPlan = SubscriptionPlan,
                Geolocation = Geolocation,
                ModifiedBy = ModifiedBy,
                ModofiedDate = ModofiedDate,
                CraetedBy = CraetedBy,
                CreatedDate = CreatedDate,
            };
        }
    }
}
