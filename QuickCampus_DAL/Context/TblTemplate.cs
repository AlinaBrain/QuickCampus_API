using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblTemplate
{
    public int Id { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ClientId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual TblClient? Client { get; set; }
}
