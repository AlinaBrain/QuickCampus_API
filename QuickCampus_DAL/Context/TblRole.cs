using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public  class TblRole
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ModofiedDate { get; set; }

    public int? ClientId { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsActive { get; set; }

    public virtual TblUser? CreatedByNavigation { get; set; }

    public virtual TblUser? ModifiedByNavigation { get; set; }

    public virtual ICollection<TblRolePermission> TblRolePermissions { get; set; } = new List<TblRolePermission>();

    public virtual ICollection<TblUserRole> TblUserRoles { get; set; } = new List<TblUserRole>();
}
