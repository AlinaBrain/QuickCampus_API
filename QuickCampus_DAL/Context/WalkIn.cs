using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class WalkIn
{
    public int WalkInId { get; set; }

    public string? Title { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public int? StateId { get; set; }

    public int? CountryId { get; set; }

    public DateTime? WalkInDate { get; set; }

    public string? JobDescription { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public int? ClientId { get; set; }

    public virtual ICollection<ApplicantTest> ApplicantTests { get; set; } = new List<ApplicantTest>();

    public virtual ICollection<CampusWalkInCollege> CampusWalkInColleges { get; set; } = new List<CampusWalkInCollege>();

    public virtual Country? Country { get; set; }

    public virtual State? State { get; set; }
}
