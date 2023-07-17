using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class Groupdl
{
    public int GroupId { get; set; }

    public string? GroupName { get; set; }

    public int? ClentId { get; set; }

    public virtual TblClient? Clent { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
