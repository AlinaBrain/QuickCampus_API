using QuickCampus_Core.Common;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Interfaces
{
    public interface ICampusRepo : IGenericRepository<WalkIn>
    {
        Task<IEnumerable<CampusGridViewModel>> GetAllCampus(int clientId);
        Task<IGeneralResult<CampusGridViewModel>> GetCampusByID(int id, int clientId);
        Task<IEnumerable<CampusGridViewModel>> Add(CampusGridViewModel campusGridViewModel);


    }
}
