using QuickCampus_Core.Common;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Interfaces
{
    public interface IRoleRepo : IGenericRepository<TblRole>
    {
        Task<IGeneralResult<string>> SetRolePermission(RoleMappingRequest roleMappingRequest);
    }
}
