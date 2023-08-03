using QuickCampus_Core.Common;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Interfaces
{
    public interface IUserRepo : IGenericRepository<TblUser>
    {
        Task Add(TblClient tblClient);
        Task<IGeneralResult<string>> DeleteRole(bool isDeleted, int id, int clientid, bool isSuperAdmin);
        Task<IGeneralResult<string>> ActiveInActiveRole(bool isActive, int id, int clientid, bool isSuperAdmin);
    }
}
