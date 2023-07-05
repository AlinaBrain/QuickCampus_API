using System;
using System.Collections.Generic;

namespace QuickCampusAPI.Context;

public partial class TblClient
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? CraetedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime ModofiedDate { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? SubscriptionPlan { get; set; }

    public string? Geolocation { get; set; }

    public virtual TblUser? CraetedByNavigation { get; set; }

    public virtual TblUser? ModifiedByNavigation { get; set; }

    public virtual ICollection<TblRole> TblRoles { get; set; } = new List<TblRole>();

    public virtual ICollection<TblUser> TblUsers { get; set; } = new List<TblUser>();
}
