using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public  class AddTblWalkinCollegeVm
    {
        public int CampusId { get; set; }

        public int? WalkInId { get; set; }

        public int? CollegeId { get; set; }

        public DateTime? StartDateTime { get; set; }

        public TimeSpan? ExamStartTime { get; set; }

        public TimeSpan? ExamEndTime { get; set; }

        public bool? IsCompleted { get; set; }

        public TblWalkInCollege ToDblWalinCollege()
        {
            return new TblWalkInCollege
            {
                WalkInId=WalkInId,
                CollegeId=CollegeId,
                StartDateTime=StartDateTime,
                ExamStartTime=ExamStartTime,
                ExamEndTime=ExamEndTime,
                IsCompleted=IsCompleted
            };
        }
        public TblWalkInCollege ToUpdateWalinCollege()
        {
            return new TblWalkInCollege
            {
                CampusId=CampusId,
                WalkInId = WalkInId,
                CollegeId = CollegeId,
                StartDateTime = StartDateTime,
                ExamStartTime = ExamStartTime,
                ExamEndTime = ExamEndTime,
                IsCompleted = IsCompleted
            };
        }
    }
}
