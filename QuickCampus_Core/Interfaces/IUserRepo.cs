using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Interfaces
{
    public interface IUserRepo : IGenericRepository<TblUser>
    {
        Task<List<TblUser>> getUserclient();
        List<RolePermissions> getPermission(int roleId, TblRole tblRole);
    }
}
