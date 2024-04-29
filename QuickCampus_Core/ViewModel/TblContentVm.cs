using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class TblContentVm
    {
        public static explicit operator TblContentVm(TblContent item)
        {
            return new TblContentVm
            {
                Id=item.Id,
                ContentTypeId=item.ContentTypeId,
                ClientId=item.ClientId,
                IsActive=item.IsActive,
                IsDeleted=item.IsDeleted,
                CreatedDate=item.CreatedDate,
                UpdatedDate=item.UpdatedDate,
            };
        }
        public int Id { get; set; }

        public int? ContentTypeId { get; set; }

        public int? ClientId { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
