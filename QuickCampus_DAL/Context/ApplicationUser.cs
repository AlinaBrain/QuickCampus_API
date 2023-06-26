using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class ApplicationUser
{
    public int ApplicationUserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Name { get; set; }

    public string? EmailId { get; set; }

    public virtual ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; } = new List<ApplicationUserRole>();
}
