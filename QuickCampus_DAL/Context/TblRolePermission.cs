using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblRolePermission
{
    public int Id { get; set; }

    public int? RoleId { get; set; }

    public int? PermissionId { get; set; }

    public virtual MstMenuSubItem? Permission { get; set; }

    public virtual TblRole? Role { get; set; }
}
