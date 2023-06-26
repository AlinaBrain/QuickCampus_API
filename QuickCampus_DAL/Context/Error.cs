using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class Error
{
    public int Id { get; set; }

    public string? Error1 { get; set; }

    public DateTime? Date { get; set; }
}
