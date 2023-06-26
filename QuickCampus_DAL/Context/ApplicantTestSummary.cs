using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class ApplicantTestSummary
{
    public int SummaryId { get; set; }

    public int? TestId { get; set; }

    public int? QuestionId { get; set; }

    public string? Answer { get; set; }

    public DateTime? SubmittedOn { get; set; }

    public bool? IsCorrect { get; set; }

    public virtual ICollection<ApplicantAnswerSummary> ApplicantAnswerSummaries { get; set; } = new List<ApplicantAnswerSummary>();

    public virtual Question? Question { get; set; }

    public virtual ApplicantTest? Test { get; set; }
}
