using System;
using System.Collections.Generic;

namespace QuickCampus_DAL.Context;

public partial class ApplicantTest
{
    public int TestId { get; set; }

    public int? CampusId { get; set; }

    public int? WalkInId { get; set; }

    public int? ApplicantId { get; set; }

    public DateTime? TestDate { get; set; }

    public TimeSpan? TestStartTime { get; set; }

    public TimeSpan? TestEndTime { get; set; }

    public int? TotalQuestion { get; set; }

    public int? TotalMarks { get; set; }

    public int? MarksObtained { get; set; }

    public int? TotalQuestionAttempted { get; set; }

    public int? TotalCorrectAnswered { get; set; }

    public int? TotalIncorrectAnswered { get; set; }

    public bool? IsTestCompeleted { get; set; }

    public int? IsDelete { get; set; }

    public virtual Applicant? Applicant { get; set; }

    public virtual ICollection<ApplicantTestSummary> ApplicantTestSummaries { get; set; } = new List<ApplicantTestSummary>();

    public virtual CampusWalkInCollege? Campus { get; set; }

    public virtual WalkIn? WalkIn { get; set; }
}
