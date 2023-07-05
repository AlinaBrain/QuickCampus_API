using System;
using System.Collections.Generic;

namespace QuickCampusAPI.Context;

public partial class Section
{
    public int SectionId { get; set; }

    public string? Section1 { get; set; }

    public int? SortOrder { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
