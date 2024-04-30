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

    public string? ForgotPassword { get; set; }

    public string? ProfilePicture { get; set; }

    public virtual TblClient? Client { get; set; }

    public virtual ICollection<TblMenuItemUserPermission> TblMenuItemUserPermissions { get; set; } = new List<TblMenuItemUserPermission>();

    public virtual ICollection<TblUserAppRole> TblUserAppRoles { get; set; } = new List<TblUserAppRole>();

    public virtual ICollection<TblUserRole> TblUserRoles { get; set; } = new List<TblUserRole>();
}
