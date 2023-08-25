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

    public virtual ICollection<Groupdl> Groupdls { get; set; } = new List<Groupdl>();

    public virtual ICollection<QuestionType> QuestionTypes { get; set; } = new List<QuestionType>();

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    public virtual ICollection<TblContent> TblContents { get; set; } = new List<TblContent>();

    public virtual ICollection<TblGoal> TblGoals { get; set; } = new List<TblGoal>();

    public virtual ICollection<TblParentSkill> TblParentSkills { get; set; } = new List<TblParentSkill>();

    public virtual ICollection<TblTag> TblTags { get; set; } = new List<TblTag>();

    public virtual ICollection<TblTopicPCChildSkill> TblTopicPCChildSkills { get; set; } = new List<TblTopicPCChildSkill>();

    public virtual ICollection<TblTopicPChildSkill> TblTopicPChildSkills { get; set; } = new List<TblTopicPChildSkill>();

    public virtual ICollection<TblUser> TblUsers { get; set; } = new List<TblUser>();
}
