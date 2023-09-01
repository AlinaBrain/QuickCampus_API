using Microsoft.AspNetCore.Http;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class OptionsVm
    {
        public int OptionId { get; set; }

        public int? QuestionId { get; set; }

        public string? OptionText { get; set; }

        public bool? IsCorrect { get; set; }

        public string? OptionImage { get; set; }

        public IFormFile Images { get; set; }
        public byte[]? Image { get; set; }

        public QuestionOption ToQuestionOptionVm()
        {
            return new QuestionOption
            {
                QuestionId= QuestionId,
                OptionText= OptionText,
                IsCorrect= true,
                OptionImage= OptionImage,
                
            };
        }
    }
}
