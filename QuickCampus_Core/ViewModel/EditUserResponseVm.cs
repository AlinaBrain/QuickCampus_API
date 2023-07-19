using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Email=item.Email,
                Mobile=item.Mobile,
                ClientId=item.ClientId,
                IsActive=item.IsActive,
                IsDelete=item.IsDelete,
            };
        }

       
        public int Id { get; set; }
        public string? UserName { get; set; }

        public string Name { get; set; }
        public string? Email { get; set; }

        public string? Mobile { get; set; }
        public int? ClientId { get; set; }
        public bool? IsDelete { get; set; }

        public bool? IsActive { get; set; }

        public TblUser ToUpdateDbModel()
        {
            return new TblUser
            {
                Id = Id,
                UserName = UserName,
                Email = Email,  
                Mobile = Mobile,
                ClientId = ClientId,
                Name = Name,
                IsActive = IsActive,
                IsDelete = IsDelete,
            };
        }
      

    }
}
