using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class EditSubTopicVm
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? TopicId { get; set; }
        public bool? IsActive { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ClientId { get; set; }

    }
}
