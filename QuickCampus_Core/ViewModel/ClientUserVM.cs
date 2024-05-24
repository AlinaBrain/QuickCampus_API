using Microsoft.Identity.Client;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.ViewModel
{
    public class ClientUserVM
    {
           public static explicit operator ClientUserVM(TblUser item)
            {
                return new ClientUserVM
                {
                    Id = item.Id,
                    UserName = item.Email,
                    Name = item.Name 
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
