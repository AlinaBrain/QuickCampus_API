using System;
using System.Collections.Generic;

namespace QuickCampusAPI.Context;

public partial class State
{
    public int StateId { get; set; }

    public string? StateName { get; set; }

    public int? CountryId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<College> Colleges { get; set; } = new List<College>();

    public virtual Country? Country { get; set; }

    public virtual ICollection<WalkIn> WalkIns { get; set; } = new List<WalkIn>();
}
