using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class MstMenuSubItem
{
    public int SubItemId { get; set; }

    public int? ItemId { get; set; }

    public string SubItemName { get; set; } = null!;

    public string SubItemDisplayName { get; set; } = null!;

    public string? SubItemIcon { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? SubItemUrl { get; set; }

    public virtual MstMenuItem? Item { get; set; }

    public virtual ICollection<TblRolePermission> TblRolePermissions { get; set; } = new List<TblRolePermission>();
}
