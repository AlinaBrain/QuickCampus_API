using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class MstCityStateCountry
{
    public int CountryId { get; set; }

    public string? CountryName { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? ClientId { get; set; }

    public virtual ICollection<MstCityState> MstCityStates { get; set; } = new List<MstCityState>();

    public virtual ICollection<TblWalkIn> TblWalkIns { get; set; } = new List<TblWalkIn>();
}
