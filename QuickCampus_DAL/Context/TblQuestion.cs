using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblQuestion
{
    public int QuestionId { get; set; }

    public int? QuestionTypeId { get; set; }

    public string? Text { get; set; }

    public int? SectionId { get; set; }

    public int? GroupId { get; set; }

    public int? Marks { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? ClientId { get; set; }

    public virtual TblClient? Client { get; set; }

    public virtual MstGroupdl? Group { get; set; }

    public virtual MstQuestionType? QuestionType { get; set; }

    public virtual MstSection? Section { get; set; }

    public virtual ICollection<TblQuestionOption> TblQuestionOptions { get; set; } = new List<TblQuestionOption>();
}
