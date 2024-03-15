using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class MstApplicationRole
{
    public int AppRoleId { get; set; }


    public string? AppRoleName { get; set; }

    public bool? Isactive { get; set; }

    public bool? IsDeleted { get; set; }
}
