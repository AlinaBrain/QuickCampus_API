using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class Applicant
{
    public int ApplicantId { get; set; }

    public string? ApplicantToken { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? EmailAddress { get; set; }

    public string? PhoneNumber { get; set; }

    public string? HigestQualification { get; set; }

    public decimal? HigestQualificationPercentage { get; set; }

    public decimal? MatricPercentage { get; set; }

    public decimal? IntermediatePercentage { get; set; }

    public string? Skills { get; set; }

    public int? StatusId { get; set; }

    public string? Comment { get; set; }

    public DateTime? RegisteredDate { get; set; }

    public int? AssignedToCompany { get; set; }

    public int? CollegeId { get; set; }

    public string? CollegeName { get; set; }

    public virtual ICollection<ApplicantComment> ApplicantComments { get; set; } = new List<ApplicantComment>();

    public virtual ICollection<ApplicantTest> ApplicantTests { get; set; } = new List<ApplicantTest>();

    public virtual Company? AssignedToCompanyNavigation { get; set; }

    public virtual Status? Status { get; set; }
}
