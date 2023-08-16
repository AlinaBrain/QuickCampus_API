using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class College
{
    public int CollegeId { get; set; }

    public string? CollegeName { get; set; }

    public string? CollegeCode { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public int? StateId { get; set; }

    public int? CountryId { get; set; }

    public string? Logo { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public string? ContectPerson { get; set; }

    public string? ContectPhone { get; set; }

    public string? ContectEmail { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; } 

    public int? ClientId { get; set; }

    public virtual ICollection<CampusWalkInCollege> CampusWalkInColleges { get; set; } = new List<CampusWalkInCollege>();

    public virtual Country? Country { get; set; }

    public virtual State? State { get; set; }
}
