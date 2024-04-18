using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class SubTopicVm
    {
        public static explicit operator SubTopicVm(TblSubTopic item)
        {
            return new SubTopicVm
            {
                Id= item.Id,
                Name= item.Name,
                TopicId= item.TopicId,
                IsActive= item.IsActive,
                ClientId= item.ClientId,
            };
        }
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? TopicId { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ClientId { get; set; }

    }
}
