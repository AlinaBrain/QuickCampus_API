using QuickCampus_Core.Common;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Interfaces
{
    public interface IRoleRepo : IGenericRepository<TblRole>
    {
        Task<IGeneralResult<string>> SetRolePermission(RoleMappingRequest roleMappingRequest);
        Task<IGeneralResult<string>> ActiveInActiveRole(bool isActive, int id, int clientid, bool isSuperAdmin);
        Task<IGeneralResult<string>> DeleteRole(int id, int clientid, bool isSuperAdmin);
        Task<IGeneralResult<string>> UpdateRole(RoleModel roleModel, int clientid, bool isSuperAdmin);
        Task<IGeneralResult<RoleModel>> GetRoleById(int rId, int clientid, bool isSuperAdmin);
    }
}
