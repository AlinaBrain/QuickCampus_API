using System;
using System.Collections.Generic;

namespace QuickCampusAPI.Context;

public partial class Company
{
    public int CompanyId { get; set; }

    public string? CompanyName { get; set; }

    public virtual ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
}
