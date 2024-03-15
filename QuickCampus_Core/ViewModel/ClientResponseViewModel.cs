using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class ClientResponseViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool? IsActive { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? SubscriptionPlan { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }
        public string? RoleName { get; set; }
        public string? AppRoleName { get; set; }

    }
}
