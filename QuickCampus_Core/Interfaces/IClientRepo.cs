using QuickCampus_Core.Common;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Interfaces
{
    public interface IClientRepo : IGenericRepository<TblClient>
    {
        string GetClientRoleName(int ClientId);
        string GetClientAppRoleName(int ClientId);
    }
}
