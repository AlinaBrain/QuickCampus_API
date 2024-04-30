

using Microsoft.AspNetCore.Http;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.ViewModel
{
    public class ProfileChangeVm
    {
        public static explicit operator ProfileChangeVm(TblUser item)
        {
            return new ProfileChangeVm
            {
                Id=item.Id,
                Name=item.Name,
            
                Mobile = item.Mobile,   
            };
        }
        public int Id { get; set; }
        public string ?Mobile { get; set; }
        public string ?Name { get; set; }
        
        public IFormFile? ImagePath { get; set; }
    }
}
