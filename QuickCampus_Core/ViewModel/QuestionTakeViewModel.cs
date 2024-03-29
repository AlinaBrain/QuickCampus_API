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
        public int QuestionTypeId { get; set; }
        public int SectionId { get; set; }
        public int GroupId { get; set; }
        public string? Text { get; set; }
        public int? Marks { get; set; }
        public bool? IsActive { get; set; }
        public List<QuestionsOptionVm> QuestionssoptionVm { get; set; }  
        
    }
}
