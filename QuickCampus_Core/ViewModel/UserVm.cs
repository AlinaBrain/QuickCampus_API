using Microsoft.AspNetCore.Mvc;
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
                ClientId = item.ClientId,
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
        public int?ClientId { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required, MaxLength(50)]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(8, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 8 characters.")]
        public string? Password { get; set; }

        public bool? IsDelete { get; set; }

        public bool? IsActive { get; set; }
        [Required, MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
        [Remote("EmailExist", "User", AdditionalFields = "Id", ErrorMessage = ("Email already exist!"))]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
        [Required,MaxLength(10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be a 10-digit number.")]
        public string? Mobile { get; set; }


        public TblUser toUserDBModel()
        {
            return new TblUser
            {
                Id = Id,
                ClientId= ClientId,
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
