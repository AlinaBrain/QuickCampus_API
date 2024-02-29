using QuickCampus_DAL.Context;

namespace QuickCampus_Core.ViewModel
{
    public  class UserResponseVm
    {
        public static explicit operator UserResponseVm(TblUser items)
        {
            return new UserResponseVm
            {
                Id = items.Id,
                Name = items.Name,
                Email = items.Email,
                ClientId = items.ClientId, 
                Mobile = items.Mobile,
                CreateDate = items.CreateDate,
                ModifiedDate = items.ModifiedDate,
                IsActive = items.IsActive,
                IsDelete = items.IsDelete
            };
            }
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Mobile { get; set; }

        public int? ClientId { get; set; }
        public DateTime? CreateDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
         public bool? IsActive { get; set; }
         public   bool? IsDelete { get; set; }
    }
}
