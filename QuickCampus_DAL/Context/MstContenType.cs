using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class MstContenType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<TblContent> TblContents { get; set; } = new List<TblContent>();
}
