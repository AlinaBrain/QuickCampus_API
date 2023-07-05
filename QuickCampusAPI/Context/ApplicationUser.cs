using System;
using System.Collections.Generic;

namespace QuickCampusAPI.Context;

public partial class ApplicationUser
{
    public int ApplicationUserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Token { get; set; }

    public virtual ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; } = new List<ApplicationUserRole>();
}
