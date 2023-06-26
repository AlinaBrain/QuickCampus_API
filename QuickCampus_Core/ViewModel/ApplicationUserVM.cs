using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class ApplicationUserVM
    {
        public string UserName { get; private set; }
        public string Password { get; private set; }

        public static explicit operator ApplicationUserVM(ApplicationUser items)
        {
            return new ApplicationUserVM
            {
                UserName = items.UserName,
                Password = items.Password,

            };
        }
        public ApplicationUser ToUserRegistrationDbModel()
        {
            return new ApplicationUser
            {
                UserName = UserName,
                Password = Password,
                
            };
        }
    }
}
