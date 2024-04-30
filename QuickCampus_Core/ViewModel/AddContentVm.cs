using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class AddContentVm
    {
        public static explicit operator AddContentVm(TblContent item)
        {
            return new AddContentVm
            {
                Id = item.Id,
                Content = item.Content,
                ContentTypeId= item.ContentTypeId,
                ClientId=item.ClientId,
                CreatedDate=item.CreatedDate,
                IsActive=item.IsActive,
            };
        }
        public int Id { get; set; }

        public int? ContentTypeId { get; set; }

        public int? ClientId { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? Content { get; set; }

    }
}
