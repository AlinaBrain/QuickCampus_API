using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class QuestionTakeViewModel
    {
        
        public int QuestionId { get; set; }
        [Required]
        public int QuestionTypeId { get; set; }
        public int SectionId { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        [Display(Name = "Question Name")]
        [MaxLength(1000, ErrorMessage = "can't exceed more than 1000 characters.")]
        public string? Text { get; set; }
        [Required]
        public int? Marks { get; set; }
        public bool? IsActive { get; set; }
        public List<QuestionsOptionVm> QuestionssoptionVm { get; set; }  
        
    }
}
