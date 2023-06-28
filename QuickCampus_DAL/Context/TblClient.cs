using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblClient
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? CraetedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime ModofiedDate { get; set; }

    public virtual TblUser? CraetedByNavigation { get; set; }

    public virtual TblUser? ModifiedByNavigation { get; set; }
}
