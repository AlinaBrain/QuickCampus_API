using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
     public  class QuestionVmm
    {
        public static explicit operator QuestionVmm(Question items)
        {
            return new QuestionVmm()
            {
                QuestionId = items.QuestionId,
                QuestionTypeId = items.QuestionTypeId,
                Text=items.Text,
                SectionId = items.SectionId,
                GroupId = items.GroupId,
                IsActive = items.IsActive,
                Marks = items.Marks,
                IsDeleted = items.IsDeleted,
                ClientId = items.ClentId,
            };
        }

        public int QuestionId { get; set; }

        public int? QuestionTypeId { get; set; }

        public string? Text { get; set; }

        public int? SectionId { get; set; }

        public int? GroupId { get; set; }

        public int? Marks { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public int? ClientId { get; set; }


        public Question ToQuestionDbModel()
        {
            return new Question
            {
                QuestionTypeId = QuestionTypeId,
                Text = Text,
                SectionId = SectionId,
                GroupId = GroupId,
                IsActive = true,
                Marks = Marks,
                IsDeleted = false,
                ClentId = ClientId,
            };
        }

        public Question ToUpdateDbModel()
        {
            return new Question
            {
                QuestionId = QuestionId,
                QuestionTypeId = QuestionTypeId,
                Text = Text,
                SectionId = SectionId,
                GroupId = GroupId,
                IsActive = true,
                Marks = Marks,
                IsDeleted = false,
                ClentId = ClientId,
            };
        }
    }
}

