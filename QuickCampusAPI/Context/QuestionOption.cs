using System;
using System.Collections.Generic;

namespace QuickCampusAPI.Context;

public partial class QuestionOption
{
    public int OptionId { get; set; }

    public int? QuestionId { get; set; }

    public string? OptionText { get; set; }

    public bool? IsCorrect { get; set; }

    public string? OptionImage { get; set; }

    public byte[]? Image { get; set; }

    public virtual ICollection<ApplicantAnswerSummary> ApplicantAnswerSummaries { get; set; } = new List<ApplicantAnswerSummary>();

    public virtual Question? Question { get; set; }
}
