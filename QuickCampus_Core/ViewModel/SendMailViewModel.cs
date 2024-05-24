using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class SendMailViewModel
    {
        [EmailAddress]
        public string ReceiverEmailId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
