using QuickCampus_DAL.Context;

namespace QuickCampus_Core.ViewModel
{
    public  class UserResponseVm
    {
        public static explicit operator UserResponseVm(TblUser items)
        {
            return new UserResponseVm
            {
                Name = items.Name,
                Email = items.Email,
                ClientId = items.ClientId, 
                Mobile = items.Mobile,
            };
            }
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Mobile { get; set; }

        public int? ClientId { get; set; }

    }
}
