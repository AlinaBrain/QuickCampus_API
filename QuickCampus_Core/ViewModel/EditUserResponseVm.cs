using QuickCampus_DAL.Context;
using System.ComponentModel.DataAnnotations;

namespace QuickCampus_Core.ViewModel
{
    public class UserRequestVm
    {
        public static explicit operator UserRequestVm(TblUser item)
        {
            return new UserRequestVm
            {
                Id = item.Id,   
                Email = item.Email,
                Mobile=item.Mobile
            };
        }
       
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string? Email { get; set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Please enter a valid 10-digit mobile number that does not start with 0.")]
        public string? Mobile { get; set; }
        public DateTime? CreateDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public TblUser ToUpdateDbModel()
        {
            return new TblUser
            {
                Id = Id,
                Email = Email,
                Mobile = Mobile,
                CreateDate = Id > 0 ? null : DateTime.UtcNow,
                ModifiedDate = DateTime.Now
            };
        }

    }

    public class UserResponseVM
    {
        public static explicit operator UserResponseVM(TblUser item)
        {
            return new UserResponseVM
            {
                Id = item.Id,
                Email = item.Email,
                Mobile = item.Mobile,
                CreateDate =item.CreateDate,
                ModifiedDate = item.ModifiedDate
            };
        }

        public int Id { get; set; }
        public string? Email { get; set; }

        public string? Mobile { get; set; }
        public DateTime? CreateDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
