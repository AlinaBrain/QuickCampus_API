using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblMenuItemUserPermission
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Items { get; set; } = null!;

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual TblUser? User { get; set; }
}
