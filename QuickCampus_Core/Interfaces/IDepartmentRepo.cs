using QuickCampus_Core.Common;
using QuickCampus_Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Interfaces
{
    public interface IDepartmentRepo
    {
        Task<List<DepartmentResponseVM>> GetAllDepartments(int clientid, bool isSuperAdmin, int pageStart,int pageSize);
        Task<IGeneralResult<DepartmentVM>> AddDepartments(int clientId, int userId, DepartmentVM vm);
        Task<IGeneralResult<DepartmentVM>> UpdateDepartments(int clientId, int userId, DepartmentVM vm);
        Task<IGeneralResult<string>> ActiveInactiveDepartments(int clientId, bool isSuperAdmin, int id, bool status);
        Task<IGeneralResult<string>> DeleteDepartments(int clientId, bool isSuperAdmin, int id);
        Task<IGeneralResult<DepartmentResponseVM>> GetDepartmentsById(int clientId, bool isSuperAdmin, int id);
    }
}
