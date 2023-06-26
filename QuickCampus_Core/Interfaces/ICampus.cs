using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Interfaces
{
    public interface ICampusRepo : IGenericRepository<WalkIn>
    {
        Task<IEnumerable<CampusGridViewModel>> GetAllCampus();
        Task<IEnumerable<CampusGridViewModel>> GetCampusByID(int id);
        Task<IEnumerable<CampusGridViewModel>> Add(CampusGridViewModel campusGridViewModel);


    }
}
