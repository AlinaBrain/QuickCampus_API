using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblParentSkill
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? ClientId { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModefiedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModefiedOn { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual TblClient? Client { get; set; }

    public virtual TblUser? CreatedByNavigation { get; set; }

    public virtual TblUser? ModefiedByNavigation { get; set; }

    public virtual ICollection<TblTopicPChildSkill> TblTopicPChildSkills { get; set; } = new List<TblTopicPChildSkill>();
}
