using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace QuickCampus_Core.ViewModel
{
    public class UserModel
    {

        public int? ClientId { get; set; }
        [Required, MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "In name only characters allowed.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(8, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 8 characters.")]
        public string? Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
        [Required, MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
        [Required]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Please enter a valid 10-digit mobile number that does not start with 0.")]
        public string Mobile { get; set; }
    }
}
