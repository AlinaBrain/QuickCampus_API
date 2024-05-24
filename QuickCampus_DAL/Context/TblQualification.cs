using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblQualification
{
    public int Id { get; set; }

    public string? QualificationName { get; set; }

    public bool? IsAcive { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<TblApplicant> Applicants { get; set; } = new List<TblApplicant>();
}
