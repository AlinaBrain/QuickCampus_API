using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class QuestionsOptionVm
    {
        public int OptionId { get; set; }
        public int? QuestionId { get; set; }
        public string? OptionText { get; set; }
        public bool? IsCorrect { get; set; }
        [Required]
        [FileExtensions(Extensions = "jpg,png,jpeg", ErrorMessage = "Only JPG and PNG and jpeg files are allowed.")]
        public IFormFile? Image { get; set; }
        public string? Imagepath { get; set; }
        public int SortOrder { get; set; }
    }
}
