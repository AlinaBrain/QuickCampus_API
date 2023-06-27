using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Common
{
    public static class common
    {
        public const int TOTAL_QUESTION_IN_TEST = 30;
        public const int SECTION_COUNT = 2;
        public enum ApplicantStatus : int
        {
            Active = 1,
            Selected = 2,
            OnHold = 3,
            Recruited = 4,
            Deleted = 5,
            Inactive = 6
        }
        public enum Section : int
        {
            Logical = 1,
            Technical = 2,
            HR = 3
        }



    }
}
