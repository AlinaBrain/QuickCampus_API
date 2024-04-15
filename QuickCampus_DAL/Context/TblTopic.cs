using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class TblTopic
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? DepartmentId { get; set; }

    public int? SubjectId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ClientId { get; set; }

    public virtual TblClient? Client { get; set; }

    public virtual TblDepartment? Department { get; set; }

    public virtual TblSubject? Subject { get; set; }

    public virtual ICollection<TblSubTopic> TblSubTopics { get; set; } = new List<TblSubTopic>();
}
