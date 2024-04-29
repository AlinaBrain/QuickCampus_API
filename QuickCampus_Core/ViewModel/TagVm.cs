using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class TagVm
    {
        public static explicit operator TagVm(TblTag item)
        {
            return new TagVm
            {
                Id= item.Id,
                Name= item.Name,
                IsActive= item.IsActive,
                CreatedDate= item.CreatedDate,
                ClientId= item.ClientId,
            };
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ClientId { get; set; }
    }
}
