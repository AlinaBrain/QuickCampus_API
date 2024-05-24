using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblQuestionOption
{
    public int OptionId { get; set; }

    public int? QuestionId { get; set; }

    public string? OptionText { get; set; }

    public bool? IsCorrect { get; set; }

    public string? Imagepath { get; set; }

    public virtual TblQuestion? Question { get; set; }
}
