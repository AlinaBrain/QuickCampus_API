using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class MstMenuItem
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public string ItemDisplayName { get; set; } = null!;

    public string? ItemIcon { get; set; }

    public bool? IsDashboardItem { get; set; }

    public bool? IsMenuItem { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ItemUrl { get; set; }

    public virtual ICollection<MstMenuSubItem> MstMenuSubItems { get; set; } = new List<MstMenuSubItem>();
}
