using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblWalkInCollege
{
    public int CampusId { get; set; }

    public int? WalkInId { get; set; }

    public int? CollegeId { get; set; }

    public DateTime? StartDateTime { get; set; }

    public TimeSpan? ExamStartTime { get; set; }

    public TimeSpan? ExamEndTime { get; set; }

    public bool? IsCompleted { get; set; }

    public string CollegeCode { get; set; } = null!;

    public virtual TblCollege? College { get; set; }

    public virtual TblWalkIn? WalkIn { get; set; }
}
