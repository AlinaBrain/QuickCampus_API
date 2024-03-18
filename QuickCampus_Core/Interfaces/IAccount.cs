using QuickCampus_Core.Common;
using QuickCampus_Core.ViewModel;

namespace QuickCampus_Core.Interfaces
{
    public interface IAccount
    {
        Task<IGeneralResult<LoginResponseVM>> Login(AdminLogin adminLogin);
        Task<IGeneralResult<List<PermissionVM>>> ListPermission();
        Task<IGeneralResult<List<RoleMappingVM>>> ListRoles(int ClientId, int UserId);
    }
}
