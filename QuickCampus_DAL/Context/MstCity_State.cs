using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class MstCity_State
{
    public int StateId { get; set; }

    public string? StateName { get; set; }

    public int? CountryId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? ClientId { get; set; }

    public virtual MstCity_State_Country? Country { get; set; }

    public virtual ICollection<WalkIn> WalkIns { get; set; } = new List<WalkIn>();
}
