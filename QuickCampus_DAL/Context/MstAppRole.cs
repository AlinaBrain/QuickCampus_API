using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class MstAppRole
{
    public int AppRoleId { get; set; }

    public string? AppRoleName { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<TblUserAppRole> TblUserAppRoles { get; set; } = new List<TblUserAppRole>();
}
