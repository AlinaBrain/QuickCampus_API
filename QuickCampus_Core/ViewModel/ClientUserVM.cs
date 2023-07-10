using Microsoft.Identity.Client;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class ClientUserVM
    {
           public static explicit operator ClientUserVM(TblUser item)
            {
                return new ClientUserVM
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    Name = item.Name
                    
                    //,
                    //CategoryId = item.CategoryId,
                    //ProcessIcon = item.ProcessIcon
                };
            }
            public int ProcessId { get; set; }
            public int? CategoryId { get; set; }
            public string ProcessName { get; set; }
            public string ProcessIcon { get; set; }
            public string Steps { get; set; }
        public int Id { get; private set; }
        public string? UserName { get; set; }

        public string? Name { get; set; }

        public string? Password { get; set; }
    }
}
