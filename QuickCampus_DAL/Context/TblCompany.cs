using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblCompany
{
    public int CompanyId { get; set; }

    public string? CompanyName { get; set; }

    public bool? IsAcive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
