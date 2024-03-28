using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class GetClientById
    {
        public static explicit operator GetClientById(TblClient item)
        {
            return new GetClientById
            {
                Id = item.Id,
                Name=item.Name,
                IsActive=item.IsActive,
                Address=item.Address,
                Email=item.Email,
                Phone=item.Phone,
                SubscriptionPlan=item.SubscriptionPlan,
                Longitude=item.Longitude,
                Latitude=item.Latitude,
            };
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? SubscriptionPlan { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public int? RoleId { get; set; }
    }
}