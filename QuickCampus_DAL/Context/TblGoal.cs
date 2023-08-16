using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblGoal
{
    public int Id { get; set; }

    public string? Goal { get; set; }

    public string? TargetYear { get; set; }

    public int? ClientId { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModefiedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModefiedOn { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual TblClient? Client { get; set; }

    public virtual TblUser? CreatedByNavigation { get; set; }

    public virtual TblUser? ModefiedByNavigation { get; set; }
}
