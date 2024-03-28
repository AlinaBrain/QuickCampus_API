using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace QuickCampus_Core.ViewModel
{
    public class UserModel
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(20, ErrorMessage = "Name must be at most 20 characters long.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]*$", ErrorMessage = "Only alphabetic characters are allowed in the name.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 20 characters.")]
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
        public string? Mobile { get; set; }


        //public DateTime? CreateDate { get; set; }

        //public DateTime? ModifiedDate { get; set; }
    }
}
