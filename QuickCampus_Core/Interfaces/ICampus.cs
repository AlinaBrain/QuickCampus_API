using QuickCampus_Core.Common;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using static QuickCampus_Core.Common.common;

namespace QuickCampus_Core.Interfaces
{
    public interface ICampusRepo : IGenericRepository<WalkIn>
    {
       Task<IGeneralResult<List<CampusGridViewModel>>> GetAllCampus();
        Task<IGeneralResult<CampusGridViewModel>> GetCampusByID(int id);
        Task<IGeneralResult<CampusGridRequestVM>> AddOrUpdateCampus(CampusGridRequestVM vm);

        Task<IGeneralResult<string>> UpdateCampus(CampusGridRequestVM vm, int clientId, int userId);
        Task<IGeneralResult<CampusGridViewModel>> ActiveInActive(int id,bool status);
        Task<IGeneralResult<CampusGridViewModel>> DeleteCampus(int id);
    }
}
