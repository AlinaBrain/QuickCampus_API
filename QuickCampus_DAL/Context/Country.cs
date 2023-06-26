using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class Country
{
    public int CountryId { get; set; }

    public string? CountryName { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<College> Colleges { get; set; } = new List<College>();

    public virtual ICollection<State> States { get; set; } = new List<State>();

    public virtual ICollection<WalkIn> WalkIns { get; set; } = new List<WalkIn>();
}
