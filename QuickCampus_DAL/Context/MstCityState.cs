﻿using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class MstCityState
{
    public int StateId { get; set; }

    public string? StateName { get; set; }

    public int? CountryId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? ClientId { get; set; }

    public virtual MstCityStateCountry? Country { get; set; }

    public virtual ICollection<MstCity> MstCities { get; set; } = new List<MstCity>();

    public virtual ICollection<TblWalkIn> TblWalkIns { get; set; } = new List<TblWalkIn>();
}
