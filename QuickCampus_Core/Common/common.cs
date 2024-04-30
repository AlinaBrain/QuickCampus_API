
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
            AddApplicant = 1,
            EditApplicant = 2,
            DeleteApplicant = 3,
            ViewApplicant = 4,
            AddClient = 5,
            EditClient = 6,
            DeleteClient = 7,
            ViewClient = 8,
            AddRole = 9,
            EditRole = 10,
            DeleteRole = 11,
            ViewRole = 12,
            AddUser = 13,
            EditUser = 14,
            DeleteUser = 15,
            ViewUser = 16,
            AddColleges = 17,
            EditColleges = 18,
            DeleteColleges = 19,
            ViewColleges = 20,
            AddCampusWalkIn = 21,
            EditCampusWalkIn = 22,
            DeleteCampusWalkIn = 23,
            ViewCampusWalkIn = 24,
            AddQuestion = 25,
            EditQuestion = 26,
            DeleteQuestion = 27,
            ViewQuestion = 28,
            AddReport = 29,
            EditReport = 30,
            DeleteReport = 31,
            ViewReport = 32
        }

    }
}
