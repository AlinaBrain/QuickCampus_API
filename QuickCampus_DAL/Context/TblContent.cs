using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblContent
{
    public int Id { get; set; }

    public int? ContentTypeId { get; set; }

    public int? ClientId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Content { get; set; }

    public virtual TblClient? Client { get; set; }

    public virtual MstContenType? ContentType { get; set; }
}
