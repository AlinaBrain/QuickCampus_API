using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class Skill
{
    public int SkillId { get; set; }

    public string? SkillName { get; set; }

    public int? ApplicantId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ClientId { get; set; }

    public virtual Applicant? Applicant { get; set; }
}
