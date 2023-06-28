using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class UserModel
    {
        [Required(ErrorMessage = "UserName is required"), MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Name is required"), MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Must be between 6 and 50 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [RegularExpression(@"(?!.* )(?=.*\d)(?=.*[A-Z]).{8,15}$",
        ErrorMessage = "Please enter a password That contains 1 small alphabet, 1 capital alphabet, 1 special character.")]
        public string? Password { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a Valid email!")]
        public string? Email { get; set; }
        [Required]
        [Phone(ErrorMessage ="Please enter a valid mobile number")]
        public string? Mobile { get; set; }
    }
}
