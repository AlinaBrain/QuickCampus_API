using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblDepartment
{
    public int Id { get; set; }

    public string? DepartmentName { get; set; }

    public string? Description { get; set; }

    public int? ClientId { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModefiedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModefiedOn { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual TblUser? CreatedByNavigation { get; set; }

    public virtual ICollection<TblContent> TblContents { get; set; } = new List<TblContent>();
}
