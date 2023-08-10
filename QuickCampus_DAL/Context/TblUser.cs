using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblUser
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Password { get; set; }

    public bool? IsDelete { get; set; }

    public bool? IsActive { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public int? ClientId { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual TblClient? Client { get; set; }

    public virtual ICollection<TblRole> TblRoleCreatedByNavigations { get; set; } = new List<TblRole>();

    public virtual ICollection<TblRole> TblRoleModifiedByNavigations { get; set; } = new List<TblRole>();

    public virtual ICollection<TblUserRole> TblUserRoles { get; set; } = new List<TblUserRole>();
}
