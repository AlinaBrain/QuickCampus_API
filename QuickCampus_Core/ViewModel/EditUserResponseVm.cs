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
                UserId = item.Id,   
                Email = item.Email,
                Name = item.Name,
                Mobile=item.Mobile
            };
        }
       
        public int UserId { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Please enter a valid 10-digit mobile number that does not start with 0.")]
        public string? Mobile { get; set; }
        //public DateTime? CreateDate { get; set; }

        //public DateTime? ModifiedDate { get; set; }
        public TblUser ToUpdateDbModel()
        {
            return new TblUser
            {
                Id = UserId,
                Email = Email,
                Mobile = Mobile,
                CreateDate = UserId > 0 ? null : DateTime.UtcNow,
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
