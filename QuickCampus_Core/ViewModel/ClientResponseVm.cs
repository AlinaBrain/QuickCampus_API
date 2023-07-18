using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class ClientResponseVm
    {
        public static explicit operator ClientResponseVm(TblClient items)
        {
            return new ClientResponseVm
            {
                Id = items.Id,
                Name = items.Name,
                Address = items.Address,
                Phone = items.Phone,
                Email = items.Email,
                CraetedBy = items.CraetedBy,
                CreatedDate = items.CreatedDate,
                ModifiedBy = items.ModifiedBy,
                ModofiedDate = items.ModofiedDate,
                SubscriptionPlan = items.SubscriptionPlan,
                Latitude = items.Latitude,
                Longitude = items.Longitude,
                IsActive = items.IsActive,
              
            };
        }
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? CraetedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModofiedDate { get; set; }

        public bool? IsActive { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? SubscriptionPlan { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }
        
    }
}
