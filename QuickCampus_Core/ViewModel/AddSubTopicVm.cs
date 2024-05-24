using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class AddSubTopicVm
    {
        public static explicit operator AddSubTopicVm(TblSubTopic item)
        {
            return new AddSubTopicVm
            {
                Name = item.Name,
                TopicId = item.TopicId,
                IsActive = item.IsActive,
                CreatedDate = item.CreatedDate,
                ClientId = item.ClientId,
            };
        }
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? TopicId { get; set; }

        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ClientId { get; set; }

    }
}
