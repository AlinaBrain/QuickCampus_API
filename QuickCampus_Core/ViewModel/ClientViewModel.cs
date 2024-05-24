using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class ClientViewModel
    {
        [Required(ErrorMessage = "Name is required"), MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Company Name is required"), MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed.")]
        public string? CompanyName { get; set; }
        [Required(ErrorMessage="Address is required"),MaxLength(60)]
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
       
        [RegularExpression(@"^\d+(\.\d{1,2})?$",ErrorMessage ="Please Enter a Valid Longitude ")]
        public decimal? Longitude { get; set; }
        //[Required(ErrorMessage ="Client Role is required.")]
        //public int? RoleId { get; set; }
       
        [RegularExpression(@"^\d+(\.\d{1,2})?$",ErrorMessage ="Please Enter a Valid Latitude")]
        public decimal? Latitude { get; set; }
        
        [Required(ErrorMessage = "Password is required"), MaxLength(20)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "A minimum 8 characters password contains a combination of uppercase and lowercase letter and number are required.")]
        public string? Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public int? ClientTypeId { get; set; }
    }
}
