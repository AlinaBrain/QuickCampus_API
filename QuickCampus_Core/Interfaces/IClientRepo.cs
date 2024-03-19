using QuickCampus_Core.Common;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Interfaces
{
    public interface IClientRepo : IGenericRepository<TblClient>
    {
        IGeneralResult<string> AddMenuRoles(MenuRoleVm vm, int loggedInUser);
    }
}
