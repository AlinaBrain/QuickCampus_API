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
                UserName = item.UserName,
                Mobile=item.Mobile,
                ClientId = item.ClientId
            };
        }

       
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string? UserName { get; set; }

       // public string Name { get; set; }
       // public string? Email { get; set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Please enter a valid 10-digit mobile number that does not start with 0.")]
        public string? Mobile { get; set; }
        public int? ClientId { get; set; }
      //  public bool? IsDelete { get; set; }

      //  public bool? IsActive { get; set; }

        public TblUser ToUpdateDbModel()
        {
            return new TblUser
            {
                Id = Id,
                UserName = UserName,
                Mobile = Mobile, 
                ClientId = ClientId
            };
        }

    }
}
