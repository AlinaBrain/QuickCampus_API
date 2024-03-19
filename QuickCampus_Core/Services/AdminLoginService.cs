using QuickCampus_Core.Interfaces;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Services
{
    public class ApplicationUserService : BaseRepository<BtprojecQuickcampusContext, ApplicationUser>, IApplicationUserRepo
    {

    }
}
