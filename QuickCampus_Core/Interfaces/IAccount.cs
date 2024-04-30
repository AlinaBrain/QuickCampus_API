using QuickCampus_Core.Common;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Interfaces
{
    public interface IAccount :IGenericRepository<TblUser>
    {
        Task<IGeneralResult<LoginResponseVM>> Login(AdminLogin adminLogin);
        IGeneralResult<List<RolesItemVm>> ListPermission(bool IsAdmin);
        Task<IGeneralResult<List<RoleMappingVM>>> ListRoles(int ClientId, int UserId);
        List<RolePermissions> GetUserPermission(int RoleId);
        TblUser CheckToken(string token,string UserId);
        TblUser GetEmail(string emailId);
        string GenerateTokenForgotPassword(string EmailId, int userId);
        IGeneralResult<List<PermissionVM>> GetUserMenu(int UserId);
    }
}
