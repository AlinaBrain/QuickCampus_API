using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class AddTagVm
    {
        public static explicit operator AddTagVm(TblTag item)
        {
            return new AddTagVm
            {
                Name = item.Name,
                IsActive = item.IsActive,
                CreatedDate = item.CreatedDate,
                CreatedBy = item.CreatedBy,
                ClientId = item.ClientId,
                IsDeleted= item.IsDeleted
            };
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ClientId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
