using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class MstGroupdl
{
    public int GroupId { get; set; }

    public string? GroupName { get; set; }

    public virtual ICollection<TblQuestion> TblQuestions { get; set; } = new List<TblQuestion>();
}
