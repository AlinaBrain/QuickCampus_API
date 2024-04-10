using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class ForgetPasswordVm
    {
        [Required]
        [EmailAddress]
        public string EmailId { get; set; }
    }
}
