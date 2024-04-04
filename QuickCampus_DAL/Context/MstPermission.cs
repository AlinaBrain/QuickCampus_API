using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class MstPermission
{
    public int Id { get; set; }

    public string PermissionName { get; set; } = null!;

    public string PermissionDisplay { get; set; } = null!;

    public int ParentPermissionId { get; set; }

    public virtual ICollection<TblRolePermission> TblRolePermissions { get; set; } = new List<TblRolePermission>();
}
