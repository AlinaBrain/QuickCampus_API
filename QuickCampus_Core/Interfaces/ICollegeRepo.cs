using QuickCampus_Core.Common;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Interfaces
{
    public interface ICollegeRepo : IGenericRepository<College>
    {
        Task<IGeneralResult<string>> DeleteCollege(int id, int clientid, bool isSuperAdmin);
    }
}
