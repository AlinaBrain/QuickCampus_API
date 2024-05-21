using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public  class EditUserModel
    {
        public int userId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(20, ErrorMessage = "Name must be at most 20 characters long.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]*$", ErrorMessage = "Only alphabetic characters are allowed in the name.")]
        public string? name { get; set; }
        [Required, MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? email { get; set; }
        [Required]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Please enter a valid 10-digit mobile number that does not start with 0.")]
        public string? mobile { get; set; }
        public int? clientId { get; set; }

        public int roleId { get; set; }

        public IFormFile? imagePath { get; set; }

    }
}
