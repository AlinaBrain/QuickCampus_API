using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class QuestionOptionVm
    {
        public int OptionId { get; set; }

        public int? QuestionId { get; set; }

        public string? OptionText { get; set; }

        public bool? IsCorrect { get; set; }
       
        public string? OptionImage { get; set; }

        public byte[]? Image { get; set; }
    }
}
