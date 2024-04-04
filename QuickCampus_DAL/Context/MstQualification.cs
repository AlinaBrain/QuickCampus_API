using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class MstQualification
{
    public int QualId { get; set; }

    public string? QualName { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<TblApplicant> TblApplicants { get; set; } = new List<TblApplicant>();
}
