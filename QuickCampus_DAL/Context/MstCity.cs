using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class MstCity
{
    public int CityId { get; set; }

    public string? CityName { get; set; }

    public int? StateId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public int? ClientId { get; set; }
}
