using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class MstCity_State_Country
{
    public int CountryId { get; set; }

    public string? CountryName { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? ClientId { get; set; }

    public virtual ICollection<MstCity_State> States { get; set; } = new List<MstCity_State>();

    public virtual ICollection<WalkIn> WalkIns { get; set; } = new List<WalkIn>();
}
