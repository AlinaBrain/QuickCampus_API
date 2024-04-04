using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblApplicant
{
    public int ApplicantId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? EmailAddress { get; set; }

    public string? PhoneNumber { get; set; }

    public string? HigestQualification { get; set; }

    public double? HigestQualificationPercentage { get; set; }

    public double? MatricPercentage { get; set; }

    public double? IntermediatePercentage { get; set; }

    public int? StatusId { get; set; }

    public string? Comment { get; set; }

    public int? CollegeId { get; set; }

    public string? CollegeName { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? HighestQualification { get; set; }

    public string? PassingYear { get; set; }

    public int? ClientId { get; set; }

    public virtual TblClient? Client { get; set; }

    public virtual MstQualification? HighestQualificationNavigation { get; set; }

    public virtual MstApplicantStatus? Status { get; set; }

    public virtual ICollection<TblApplicantSkill> TblApplicantSkills { get; set; } = new List<TblApplicantSkill>();
}
