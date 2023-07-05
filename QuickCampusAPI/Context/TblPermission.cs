using System;
using System.Collections.Generic;

namespace QuickCampusAPI.Context;

public partial class TblPermission
{
    public int Id { get; set; }

    public string PermissionName { get; set; } = null!;

    public string PermissionDisplay { get; set; } = null!;

    public int ParentPermissionId { get; set; }

    public virtual ICollection<TblRolePermission> TblRolePermissions { get; set; } = new List<TblRolePermission>();
}
