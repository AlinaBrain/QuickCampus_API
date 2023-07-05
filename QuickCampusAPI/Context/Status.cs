using System;
using System.Collections.Generic;

namespace QuickCampusAPI.Context;

public partial class Status
{
    public int StatusId { get; set; }

    public string? Status1 { get; set; }

    public virtual ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
}
