using QuickCampus_Core.Common;
using QuickCampus_Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Interfaces
{
    public  interface IGoalService
    {
        Task<GeneralResult<List<GoalResponseVM>>> GetAllGoals(int clientid, bool isSuperAdmin, int pageStart, int pageSize);
        Task<IGeneralResult<GoalRequestVM>> AddGoal(int clientId, int userId, GoalRequestVM vm);
        Task<IGeneralResult<GoalRequestVM>> UpdateGoal(int clientId, int userId, GoalRequestVM vm);
        Task<IGeneralResult<string>> ActiveInactiveGoal(int clientId, bool isSuperAdmin, int id, bool status);
        Task<IGeneralResult<string>> DeleteGoal(int clientId, bool isSuperAdmin, int id);
        Task<IGeneralResult<GoalResponseVM>> GetGoalById(int clientId, bool isSuperAdmin, int id);
    }
}
