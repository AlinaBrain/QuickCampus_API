
using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

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
        public enum AppRole
        {
            None = 0,
            Admin = 1,
            Client = 2,
            Client_User = 3,
            Admin_User = 4
        }
        public enum DataTypeFilter
        {
            All = 1,
            OnlyActive = 2,
            OnlyInActive = 3
        }

        [Flags]
        public enum RolesList
        {
            AddApplicant,
            EditApplicant,
            DeleteApplicant,
            AcInApplicant,  
            AddClient,
            EditClient,
            DeleteClient,
            AcInClient,
            AddRole,
            EditRole,
            DeleteRole,
            AcInRole,
            AddUser,
            EditUser,
            DeleteUser,
            AcInUser,
            AddColleges,
            EditColleges,
            DeleteColleges,
            AcInColleges,
            AddCampusWalkIn,
            EditCampusWalkIn,
            DeleteCampusWalkIn,
            AcInCampusWalkIn,
            AddQuestion,
            EditQuestion,
            DeleteQuestion,
            AcInQuestion,
            AddReport,
            EditReport,
            DeleteReport,
            AcInReport
        }

    }
}
