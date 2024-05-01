using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblClient
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? CraetedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModofiedDate { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? SubscriptionPlan { get; set; }

    public decimal? Longitude { get; set; }

    public decimal? Latitude { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? CompanyName { get; set; }

    public int? ClientTypeId { get; set; }

    public virtual MstClientType? ClientType { get; set; }

    public virtual ICollection<TblApplicant> TblApplicants { get; set; } = new List<TblApplicant>();

    public virtual ICollection<TblContent> TblContents { get; set; } = new List<TblContent>();

    public virtual ICollection<TblDepartment> TblDepartments { get; set; } = new List<TblDepartment>();

    public virtual ICollection<TblQuestion> TblQuestions { get; set; } = new List<TblQuestion>();

    public virtual ICollection<TblSubTopic> TblSubTopics { get; set; } = new List<TblSubTopic>();

    public virtual ICollection<TblSubject> TblSubjects { get; set; } = new List<TblSubject>();

    public virtual ICollection<TblTag> TblTags { get; set; } = new List<TblTag>();

    public virtual ICollection<TblTemplate> TblTemplates { get; set; } = new List<TblTemplate>();

    public virtual ICollection<TblTopic> TblTopics { get; set; } = new List<TblTopic>();

    public virtual ICollection<TblUser> TblUsers { get; set; } = new List<TblUser>();

    public virtual ICollection<TblWalkIn> TblWalkIns { get; set; } = new List<TblWalkIn>();
}
