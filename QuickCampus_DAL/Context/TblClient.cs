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

    public virtual TblUser? CraetedByNavigation { get; set; }

    public virtual ICollection<Groupdl> Groupdls { get; set; } = new List<Groupdl>();

    public virtual TblUser? ModifiedByNavigation { get; set; }

    public virtual ICollection<QuestionType> QuestionTypes { get; set; } = new List<QuestionType>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    public virtual ICollection<TblRole> TblRoles { get; set; } = new List<TblRole>();

    public virtual ICollection<TblUser> TblUsers { get; set; } = new List<TblUser>();
}
