using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class QuestionViewModelAdmin
    {

        public int QuestionId { get; set; }
        [Required(ErrorMessage = "Question Type is required.")]
        [Display(Name = "Questiton Type")]
        public int QuestionTypeId { get; set; }
        [Required(ErrorMessage = "Section is required.")]
        [Display(Name = "Section")]
        public int SectionId { get; set; }
        [Display(Name = "Group")]
        public int GroupId { get; set; }
        [Required(ErrorMessage = "Question is required.")]
        [Display(Name = "Question")]
        public string Question { get; set; }
        public string QuestionTypeName { get; set; }
        public string QuestionSection { get; set; }
        public string QuestionGroup { get; set; }
        public int Marks { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<OptionViewModelAdmin> options { get; set; }
        public IEnumerable<SelectListItem> QuestionTypes { get; set; }
        public IEnumerable<SelectListItem> Sections { get; set; }
        public IEnumerable<SelectListItem> Groups { get; set; }
    }
    public class OptionViewModelAdmin
    {
        [DataType(DataType.Upload)]
       // public HttpPostedFileBase ImageUpload { get; set; }
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
        [RequiredIf("ImageUpload", "OptionId", ErrorMessage = "Options is required.")]
        [Display(Name = "Option")]
        //[AllowHtml]
        public string OptionText { get; set; }
        public string OptionImage { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsNew { get; set; }


    }
    public class QuestionTypeViewModelAdmin
    {

        public int QuestionTypeId { get; set; }
        public string Questiontype { get; set; }

    }
    public class SectionViewModelAdmin
    {

        public int SectionId { get; set; }
        public string SectionName { get; set; }

    }
    public class GroupViewModelAdmin
    {

        public int GroupId { get; set; }
        public string GroupName { get; set; }

    }
    public class RequiredIfAttribute : RequiredAttribute
    {
        private String FirstPropertyName { get; set; }
        private String SecondPropertyName { get; set; }

        public RequiredIfAttribute(String firstpropertyName, string secondPropertyName)
        {
            FirstPropertyName = firstpropertyName;
            SecondPropertyName = secondPropertyName;

        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(FirstPropertyName).GetValue(instance, null);
            Object proprtyvalue2 = type.GetProperty(SecondPropertyName).GetValue(instance, null);
            if (proprtyvalue != null)
            {
                return ValidationResult.Success;
            }
            else
            {
                if ((int)proprtyvalue2 == 0)
                {
                    ValidationResult result = base.IsValid(value, context);
                    return result;
                }
                else { return ValidationResult.Success; }
            }
        }
    }
}
