using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Interfaces
{
    public interface IRoleRepo : IGenericRepository<TblRole>
    {
        Task<string> SetRolePermission(RoleMappingRequest roleMappingRequest);
    }
}
