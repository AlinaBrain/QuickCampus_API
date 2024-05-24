using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class MstQuestionType
{
    public int QuestionTypeId { get; set; }

    public string? QuestionType { get; set; }

    public int? Marks { get; set; }

    public virtual ICollection<TblQuestion> TblQuestions { get; set; } = new List<TblQuestion>();
}
