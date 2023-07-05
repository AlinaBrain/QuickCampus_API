using System;
using System.Collections.Generic;

namespace QuickCampusAPI.Context;

public partial class ApplicantComment
{
    public int CommentId { get; set; }

    public int? ApplicantId { get; set; }

    public string? Description { get; set; }

    public DateTime? CommentedOn { get; set; }

    public virtual Applicant? Applicant { get; set; }
}
