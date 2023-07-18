using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public  class UserResponseVm
    {
        public static explicit operator UserResponseVm(TblUser items)
        {
            return new UserResponseVm
            {
                Id = items.Id,
                UserName = items.UserName,
                Name = items.Name,
                Email = items.Email,
                ClientId = items.ClientId,
               
                Mobile = items.Mobile,

            };
            }
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Mobile { get; set; }

        public int? ClientId { get; set; }

    }
}
