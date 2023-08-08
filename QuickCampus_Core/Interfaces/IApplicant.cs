using QuickCampus_Core.Common;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Interfaces
{
    public interface IApplicantRepo : IGenericRepository<Applicant>
    {
        Task<IGeneralResult<string>> ActiveInActiveRole(bool isActive, int id, int clientid, bool isSuperAdmin);
    }
}
