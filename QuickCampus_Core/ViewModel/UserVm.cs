using QuickCampus_DAL.Context;
using System.ComponentModel.DataAnnotations;

namespace QuickCampus_Core.ViewModel
{
    public class UserVm
    {

        public static explicit operator UserVm(TblUser item)
        {
            return new UserVm
            {
                Id = item.Id,
                UserName = item.UserName,
                Name = item.Name,
                Password = item.Password,
                IsDelete = item.IsDelete,
                IsActive = item.IsActive,
                Email = item.Email,
                Mobile = item.Mobile,
            };

        }
        public int Id { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Must be between 6 and 50 characters", MinimumLength = 8)]
        [RegularExpression(@"(?!.* )(?=.*\d)(?=.*[A-Z]).{8,15}$",
        ErrorMessage = "Please enter a password That contains 1 small alphabet, 1 capital alphabet, 1 special character.")]
        public string? Password { get; set; }

        public bool? IsDelete { get; set; }

        public bool? IsActive { get; set; }
        [Required, MaxLength(100)]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
        [Required]
        [Phone(ErrorMessage = "Please enter a valid mobile number")]
        public string? Mobile { get; set; }


        public TblUser toUserDBModel()
        {
            return new TblUser
            {
                Id = Id,
                UserName = UserName,
                Name = Name,
                Password = Password,
                Email = Email,
                Mobile = Mobile,
                IsDelete = false,
                IsActive = true,
            };
        }
    }
}
