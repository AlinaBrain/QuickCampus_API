using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class QuestionType
{
    public int QuestionTypeId { get; set; }

    public string? QuestionType1 { get; set; }

    public int? Marks { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
