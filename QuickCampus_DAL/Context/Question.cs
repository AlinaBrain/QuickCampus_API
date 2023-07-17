using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class Question
{
    public int QuestionId { get; set; }

    public int? QuestionTypeId { get; set; }

    public string? Text { get; set; }

    public int? SectionId { get; set; }

    public int? GroupId { get; set; }

    public int? Marks { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? ClentId { get; set; }

    public virtual ICollection<ApplicantTestSummary> ApplicantTestSummaries { get; set; } = new List<ApplicantTestSummary>();

    public virtual TblClient? Clent { get; set; }

    public virtual Groupdl? Group { get; set; }

    public virtual ICollection<QuestionOption> QuestionOptions { get; set; } = new List<QuestionOption>();

    public virtual QuestionType? QuestionType { get; set; }

    public virtual Section? Section { get; set; }
}
