using Microsoft.AspNetCore.Http;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class UserViewVm
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public int? ClientId { get; set; }
        public bool? IsActive { get; set; }

        public string? ProfilePicture { get; set; }


        public static explicit operator UserViewVm(TblUser v)
        {
            return new UserViewVm
            {
                UserId = v.Id,
                Name = v.Name,
                Email = v.Email,
                Mobile = v.Mobile,
                ClientId = v.ClientId,
                IsActive = v.IsActive,
                
            };
        }
    }
}
