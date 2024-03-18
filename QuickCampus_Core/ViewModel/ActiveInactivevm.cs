using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class ActiveInactivevm
    {
        public static explicit operator ActiveInactivevm(TblClient item)
        {
            return new ActiveInactivevm
            {
                Id = item.Id,
                Name = item.Name,
                Address= item.Address,
                Phone = item.Phone,
                Email= item.Email,
                SubscriptionPlan=item.SubscriptionPlan,
                IsActive=item.IsActive
            };
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? SubscriptionPlan { get; set; }
        public bool? IsActive { get; set; }
    }
}
