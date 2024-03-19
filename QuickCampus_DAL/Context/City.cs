using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class City
{
    public int CityId { get; set; }

    public string? CityName { get; set; }

    public int? StateId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public int? ClientId { get; set; }

    public virtual ICollection<College> Colleges { get; set; } = new List<College>();

    public virtual State? State { get; set; }

    public virtual ICollection<WalkIn> WalkIns { get; set; } = new List<WalkIn>();
}
