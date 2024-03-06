using QuickCampus_Core.Common;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Interfaces
{
    public interface ICampusRepo : IGenericRepository<WalkIn>
    {
        Task<IEnumerable<CampusGridViewModel>> GetAllCampus(int clientId, bool isSuperAdmin, int pageStart, int pageSize);
        Task<CampusGridViewModel> GetCampusByID(int id, int clientId, bool isSuperAdmin);
        Task<IGeneralResult<string>> AddCampus(CampusGridRequestVM vm, int clientId, int userId);

        Task<IGeneralResult<string>> UpdateCampus(CampusGridRequestVM vm, int clientId, int userId);
        Task<IGeneralResult<CampusGridViewModel>> UpdateCampusStatus(int id, int clientId, bool status, bool isSuperAdmin);
        Task<IGeneralResult<CampusGridViewModel>> DeleteCampus(int id, int clientId, bool isSuperAdmin);
    }
}
