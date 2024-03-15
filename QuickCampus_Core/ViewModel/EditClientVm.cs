using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class EditClientVm
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required"), MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed.")]
        public string? Name { get; set; }
        public string? Address { get; set; }
        [Required]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Please enter a valid 10-digit mobile number that does not start with 0.")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "You must provide an email address.")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Only Email allowed.")]
        [MaxLength(100, ErrorMessage = "can't exceed more than 100 characters.")]
        [EmailAddress(ErrorMessage = "Not a valid email address.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Subscription plan is required"), MaxLength(20)]
        public string? SubscriptionPlan { get; set; }
        [Required(ErrorMessage = "Longitude is required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? Longitude { get; set; }
        [Required(ErrorMessage = "Longitude is required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? Latitude { get; set; }
        
    } 
}
