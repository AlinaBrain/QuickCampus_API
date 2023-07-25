using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Interfaces
{
    public interface ICollegeRepo : IGenericRepository<College>
    {
       Task <IEnumerable<CollegeVM>> GetAllCollege();
    }
}
