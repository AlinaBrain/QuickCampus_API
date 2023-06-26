using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class ApplicationRole
{
    public int ApplicationRoleId { get; set; }

    public string Role { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; } = new List<ApplicationUserRole>();
}
