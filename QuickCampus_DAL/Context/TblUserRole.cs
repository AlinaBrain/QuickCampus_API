using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblUserRole
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? RoleId { get; set; }

    public virtual TblRole? Role { get; set; }

    public virtual TblUser? User { get; set; }
}
