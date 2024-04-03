using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class MstSkill
{
    public int SkillId { get; set; }

    public string? SkillName { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ClientId { get; set; }
}
