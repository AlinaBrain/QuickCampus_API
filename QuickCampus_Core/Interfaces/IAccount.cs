using QuickCampus_Core.Common;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Interfaces
{
    public interface IAccount
    {
        Task<IGeneralResult<LoginResponseVM>> Login(AdminLogin adminLogin);
        Task<IGeneralResult<List<PermissionVM>>> ListPermission();
        Task<IGeneralResult<List<RoleMappingVM>>> ListRoles();

        //Task<IGeneralResult<RoleMappingVM>> GetRolePermissionById(int [] roleIds);
    }
}
