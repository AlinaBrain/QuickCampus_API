using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class MstClientType
{
    public int Id { get; set; }

    public string? TypeName { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<TblClient> TblClients { get; set; } = new List<TblClient>();
}
