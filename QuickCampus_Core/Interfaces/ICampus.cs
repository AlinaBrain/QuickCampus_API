using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Interfaces
{
    public interface ICampusRepo : IGenericRepository<WalkIn>
    {
        Task<IEnumerable<CampusGridViewModel>> GetAllCampus();
        Task<CampusGridViewModel> GetCampusByID(int id);
        Task<IEnumerable<CampusGridViewModel>> Add(CampusGridViewModel campusGridViewModel);


    }
}
