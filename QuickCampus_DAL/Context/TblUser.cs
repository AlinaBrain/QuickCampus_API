using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblUser
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Password { get; set; }

    public bool? IsDelete { get; set; }

    public bool? IsActive { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public int? ClientId { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual TblClient? Client { get; set; }

    public virtual ICollection<TblContent> TblContentCreatedByNavigations { get; set; } = new List<TblContent>();

    public virtual ICollection<TblContent> TblContentModefiedByNavigations { get; set; } = new List<TblContent>();

    public virtual ICollection<TblDepartment> TblDepartmentCreatedByNavigations { get; set; } = new List<TblDepartment>();

    public virtual ICollection<TblDepartment> TblDepartmentModefiedByNavigations { get; set; } = new List<TblDepartment>();

    public virtual ICollection<TblGoal> TblGoalCreatedByNavigations { get; set; } = new List<TblGoal>();

    public virtual ICollection<TblGoal> TblGoalModefiedByNavigations { get; set; } = new List<TblGoal>();

    public virtual ICollection<TblParentSkill> TblParentSkillCreatedByNavigations { get; set; } = new List<TblParentSkill>();

    public virtual ICollection<TblParentSkill> TblParentSkillModefiedByNavigations { get; set; } = new List<TblParentSkill>();

    public virtual ICollection<TblRole> TblRoleCreatedByNavigations { get; set; } = new List<TblRole>();

    public virtual ICollection<TblRole> TblRoleModifiedByNavigations { get; set; } = new List<TblRole>();

    public virtual ICollection<TblTag> TblTagCreatedByNavigations { get; set; } = new List<TblTag>();

    public virtual ICollection<TblTag> TblTagModefiedByNavigations { get; set; } = new List<TblTag>();

    public virtual ICollection<TblTopicPCChildSkill> TblTopicPCChildSkillCreatedByNavigations { get; set; } = new List<TblTopicPCChildSkill>();

    public virtual ICollection<TblTopicPCChildSkill> TblTopicPCChildSkillModefiedByNavigations { get; set; } = new List<TblTopicPCChildSkill>();

    public virtual ICollection<TblTopicPChildSkill> TblTopicPChildSkillCreatedByNavigations { get; set; } = new List<TblTopicPChildSkill>();

    public virtual ICollection<TblTopicPChildSkill> TblTopicPChildSkillModefiedByNavigations { get; set; } = new List<TblTopicPChildSkill>();

    public virtual ICollection<TblUserRole> TblUserRoles { get; set; } = new List<TblUserRole>();
}
