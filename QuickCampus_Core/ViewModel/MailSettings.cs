using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string FromUserName { get; set; }
        public string Password { get; set; }
        public string SendGridApiKey { get; set; }
        public string FromEmail { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
