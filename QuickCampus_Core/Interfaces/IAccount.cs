using QuickCampus_Core.Common;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Interfaces
{
    public interface IAccount
    {
        Task<IGeneralResult<LoginResponseVM>> Login(AdminLogin adminLogin);
        Task<IGeneralResult<List<PermissionVM>>> ListPermission();
        Task<IGeneralResult<List<RoleMappingVM>>> ListRoles(int ClientId, int UserId);
        public List<RolePermissions> GetUserPermission(int RoleId);

        Task<TblUser> GetEmail(string emailId);
    }
}
