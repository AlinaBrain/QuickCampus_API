using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblApplicantSkill
{
    public int SkillId { get; set; }

    public int? ApplicantId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ClientId { get; set; }

    public int? ApplicantSkillId { get; set; }

    public virtual TblApplicant? Applicant { get; set; }

    public virtual MstSkill? ApplicantSkill { get; set; }
}
