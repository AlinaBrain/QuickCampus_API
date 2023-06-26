using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class Company
{
    public int CompanyId { get; set; }

    public string? CompanyName { get; set; }

    public virtual ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
}
