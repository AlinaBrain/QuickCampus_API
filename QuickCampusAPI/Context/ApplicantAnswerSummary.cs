using System;
using System.Collections.Generic;

namespace QuickCampusAPI.Context;

public partial class ApplicantAnswerSummary
{
    public int AnswerSummaryId { get; set; }

    public int? SummaryId { get; set; }

    public int? AnswerId { get; set; }

    public virtual QuestionOption? Answer { get; set; }

    public virtual ApplicantTestSummary? Summary { get; set; }
}
