using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Services
{
    public class GoalService : IGoalService
    {
        private readonly QuikCampusDevContext _context;
        public GoalService(QuikCampusDevContext context)
        {
            _context = context;
        }
        public async Task<GeneralResult<List<GoalResponseVM>>> GetAllGoals(int clientid, bool isSuperAdmin, int pageStart, int pageSize)
        {
            GeneralResult<List<GoalResponseVM>> response = new GeneralResult<List<GoalResponseVM>>();

            if (isSuperAdmin)
            {
                response.Data = _context.TblGoals.Where(w => w.IsDeleted != true && (clientid == 0 ? true : w.ClientId == clientid)).Select(s =>
                  new GoalResponseVM()
                  {
                      Id = s.Id,
                      Goal = s.Goal,
                      targetYear= s.TargetYear,
                      IsActive=s.IsActive
                  }).ToList();
            }
            else
            {
                response.Data = _context.TblGoals.Where(w => w.IsDeleted != true && w.ClientId == clientid).Select(s =>
                 new GoalResponseVM()
                 {
                     Id = s.Id,
                     Goal = s.Goal,
                     targetYear = s.TargetYear,
                     IsActive = s.IsActive
                 }).ToList();
            }
            if (response.Data.Count() == 0)
            {
                response.IsSuccess = true;
                response.Message = "No Record Found";
            }
            else
            {
                response.IsSuccess = true;
                response.Message = "Record Fatch Successfully";
            }

            return response;
        }
        public async Task<IGeneralResult<GoalRequestVM>> AddGoal(int clientId, int userId, GoalRequestVM vm)
        {
            IGeneralResult<GoalRequestVM> result = new GeneralResult<GoalRequestVM>();

            bool isExist = _context.TblGoals.Any(s => s.Goal == vm.Goal);
            if (isExist)
            {
                result.IsSuccess = false;
                result.Message = "Goal Already Exist";
                result.Data = vm;
                return result;
            }

            TblGoal request = new TblGoal()
            {
                ClientId = clientId,
                Goal = vm.Goal,
                TargetYear = vm.targetYear,
                CreatedOn = DateTime.Now,
                ModefiedBy = null,
                ModefiedOn = null,
                IsDeleted = false,
                IsActive = true,
                CreatedBy = userId
            };

            _context.TblGoals.Add(request);
            int save = _context.SaveChanges();
            if (save > 0)
            {
                result.IsSuccess = true;
                result.Message = "Goal Added Exist";
                result.Data = vm;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Something Went Wrong";
                result.Data = vm;
            }
            return result;
        }
        public async Task<IGeneralResult<GoalRequestVM>> UpdateGoal(int clientId, int userId, GoalRequestVM vm)
        {
            IGeneralResult<GoalRequestVM> result = new GeneralResult<GoalRequestVM>();

            var rec = _context.TblGoals.Where(s => s.Goal== vm.Goal&& s.Id != vm.Id).FirstOrDefault();
            if (rec == null)
            {
                result.IsSuccess = false;
                result.Message = "Goal Already Exist";
                result.Data = vm;
                return result;
            }

            rec.Goal = vm.Goal;
            rec.TargetYear = vm.targetYear;
            rec.ModefiedBy = userId;
            rec.ModefiedOn = DateTime.Now;


            _context.TblGoals.Update(rec);
            int save = _context.SaveChanges();
            if (save > 1)
            {
                result.IsSuccess = true;
                result.Message = "Goal update successfully";
                result.Data = vm;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Something Went Wrong";
                result.Data = vm;
            }
            return result;
        }
        public async Task<IGeneralResult<string>> ActiveInactiveGoal(int clientId, bool isSuperAdmin, int id, bool status)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            TblGoal? goal = new TblGoal();
            if (isSuperAdmin)
            {
                goal= _context.TblGoals.Where(w => w.Id == id && w.IsDeleted != true && (clientId == 0 ? true : w.ClientId == clientId)).FirstOrDefault();
            }
            else
            {
                goal = _context.TblGoals.Where(w => w.Id == id && w.IsDeleted != true && w.ClientId == clientId).FirstOrDefault();
            }

            if (goal == null)
            {
                result.IsSuccess = false;
                result.Message = "Goal Not Found";
                result.Data = null;
                return result;
            }

            goal.IsActive = status;
            _context.TblGoals.Update(goal);
            int update = _context.SaveChanges();

            if (update > 1)
            {
                result.IsSuccess = true;
                result.Message = "Goal Update successfully";
                result.Data = null;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Something Went Wrong";
                result.Data = null;
            }
            return result;
        }
        public async Task<IGeneralResult<string>> DeleteGoal(int clientId, bool isSuperAdmin, int id)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            TblGoal? goal = new TblGoal();
            if (isSuperAdmin)
            {
                goal = _context.TblGoals.Where(w => w.Id == id && w.IsDeleted != true && (clientId == 0 ? true : w.ClientId == clientId)).FirstOrDefault();
            }
            else
            {
                goal = _context.TblGoals.Where(w => w.Id == id && w.IsDeleted != true && w.ClientId == clientId).FirstOrDefault();
            }

            if (goal == null)
            {
                result.IsSuccess = false;
                result.Message = "Goal Not Found";
                result.Data = null;
                return result;
            }

            goal.IsDeleted = true;
            _context.TblGoals.Update(goal);
            int update = _context.SaveChanges();

            if (update > 1)
            {
                result.IsSuccess = true;
                result.Message = "Goal delete Successfully";
                result.Data = null;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Something Went Wrong";
                result.Data = null;
            }
            return result;
        }
        public async Task<IGeneralResult<GoalResponseVM>> GetGoalById(int clientId, bool isSuperAdmin, int id)
        {
            IGeneralResult<GoalResponseVM> response = new GeneralResult<GoalResponseVM>();
            TblGoal? goal = new TblGoal();
            if (isSuperAdmin)
            {
                goal = _context.TblGoals.Where(w => w.Id == id && w.IsDeleted != true && w.IsActive == true && (clientId == 0 ? true : w.ClientId == clientId)).FirstOrDefault();
            }
            else
            {
                goal = _context.TblGoals.Where(w => w.Id == id && w.IsDeleted != true && w.IsActive == true && w.ClientId == clientId).FirstOrDefault();
            }

            if (goal != null)
            {
                response.Message = "No record found";
                response.IsSuccess = false;
                response.Data = null;
                return response;
            }
            GoalResponseVM vm = new GoalResponseVM()
            {
                Id = goal.Id,
                Goal = goal.Goal,
                targetYear = goal.TargetYear,
                IsActive=goal.IsActive
            };
            response.Message = "Record fetch successfully";
            response.IsSuccess = true;
            response.Data = vm;

            return response;
        }
    }
}
