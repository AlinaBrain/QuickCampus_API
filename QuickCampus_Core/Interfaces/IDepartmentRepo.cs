using QuickCampus_Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Interfaces
{
    internal interface IDepartmentRepo
    {
        Task<List<DepartmentVM>> GetAllDepartments(int clientid, bool isSuperAdmin);

    }
}
