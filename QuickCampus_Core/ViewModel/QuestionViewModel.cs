using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public  class QuestionViewModel
    {
        public static explicit operator QuestionViewModel(TblQuestion item)
        {
            return new QuestionViewModel
            {
                QuestionId = item.QuestionId,
                QuestionTypeId = item.QuestionTypeId,
                Text=item.Text,
                Marks=item.Marks,
                IsActive=item.IsActive,
                SectionId=item.SectionId,
                GroupId=item.GroupId,
                IsDeleted=item.IsDeleted,

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
    }
}
