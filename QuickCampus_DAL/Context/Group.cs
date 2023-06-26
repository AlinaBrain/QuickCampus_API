using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class Group
{
    public int GroupId { get; set; }

    public string? GroupName { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
