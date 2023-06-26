using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class ApplicationUserRole
{
    public int Id { get; set; }

    public int ApplicationUserId { get; set; }

    public int ApplicationRoleId { get; set; }

    public virtual ApplicationRole ApplicationRole { get; set; } = null!;

    public virtual ApplicationUser ApplicationUser { get; set; } = null!;
}
