
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
            Admin = 1,
            Client = 2,
            Client_User = 3
        }


    }
}
