using QuickCampus_DAL.Context;
using System.ComponentModel.DataAnnotations;

namespace QuickCampus_Core.ViewModel
{
    public class EditUserResponseVm
    {
        public static explicit operator EditUserResponseVm(TblUser item)
        {
            return new EditUserResponseVm
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
}
