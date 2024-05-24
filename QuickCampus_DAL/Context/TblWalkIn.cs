using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblWalkIn
{
    public int WalkInId { get; set; }

    public string? Title { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public int? StateId { get; set; }

    public int? CountryId { get; set; }

    public DateTime? WalkInDate { get; set; }

    public string? JobDescription { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public int? City { get; set; }

    public string? PassingYear { get; set; }

    public int? ClientId { get; set; }

    public virtual MstCity? CityNavigation { get; set; }

    public virtual TblClient? Client { get; set; }

    public virtual MstCityStateCountry? Country { get; set; }

    public virtual MstCityState? State { get; set; }

    public virtual ICollection<TblWalkInCollege> TblWalkInColleges { get; set; } = new List<TblWalkInCollege>();
}
