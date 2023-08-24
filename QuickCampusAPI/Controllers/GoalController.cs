using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GoalController : Controller
    {
        private readonly IGoalService goalRepo;
        private IConfiguration _config;
        public GoalController(IGoalService goalRepo, IConfiguration config)
        {
            this.goalRepo= goalRepo;
            _config = config;
        }

        //[Authorize(Roles = "GetAllGoal")]
        [HttpGet]
        [Route("managegoal")]

        public async Task<IActionResult> ManageGoal(int clientid, int pageStart = 0, int pageSize = 10)
        {
            IGeneralResult<List<GoalResponseVM>> result = new GeneralResult<List<GoalResponseVM>>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            int getClientId = 0;

            if (!isSuperAdmin && clientId == "0")
            {
                result.Data = null;
                result.IsSuccess = false;
                result.Message = "Please enter clientID";
            }

            if (isSuperAdmin)
            {
                getClientId = (int)clientid;
            }
            else
            {
                getClientId = string.IsNullOrEmpty(clientId) == true ? 0 : Convert.ToInt32(clientId);
            }

            var res = await goalRepo.GetAllGoals(getClientId, isSuperAdmin, pageStart, pageSize);

            return Ok(res);
        }



        //[Authorize(Roles = "AddGoal")]
        [HttpPost]
        [Route("addgoal")]
        public async Task<IActionResult> AddOrUpdateGoal(int clientid, [FromBody] GoalRequestVM vm)
        {
            IGeneralResult<List<GoalResponseVM>> result = new GeneralResult<List<GoalResponseVM>>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            int getClientId = 0;

            if (!isSuperAdmin && clientId == "0")
            {
                result.Data = null;
                result.IsSuccess = false;
                result.Message = "Please enter clientID";
            }

            if (isSuperAdmin)
            {
                getClientId = (int)clientid;
            }
            else
            {
                getClientId = string.IsNullOrEmpty(clientId) == true ? 0 : Convert.ToInt32(clientId);
            }
            if (vm.Id == 0)
            {
                var res = await goalRepo.AddGoal(clientid, Convert.ToInt32(userId), vm);
                return Ok(res);
            }
            else
            {
                var res = await goalRepo.UpdateGoal(getClientId, Convert.ToInt32(userId), vm);
                return Ok(res);
            }
        }

        //[Authorize(Roles = "ActiveInactiveGoal")]
        [HttpPost]
        [Route("activeinactivegoal")]
        public async Task<IActionResult> ActiveInactiveGoal(int clientid, int id, bool status)
        {
            IGeneralResult<List<GoalResponseVM>> result = new GeneralResult<List<GoalResponseVM>>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            int getClientId = 0;

            if (!isSuperAdmin && clientId == "0")
            {
                result.Data = null;
                result.IsSuccess = false;
                result.Message = "Please enter clientID";
            }

            if (isSuperAdmin)
            {
                getClientId = (int)clientid;
            }
            else
            {
                getClientId = string.IsNullOrEmpty(clientId) == true ? 0 : Convert.ToInt32(clientId);
            }

            var res = await goalRepo.ActiveInactiveGoal(getClientId, isSuperAdmin, id, status);
            return Ok(res);

        }


        //[Authorize(Roles = "DeleteGoal")]
        [HttpPost]
        [Route("deletegoal")]
        public async Task<IActionResult> DeleteGoal(int clientid, int id)
        {
            IGeneralResult<List<GoalResponseVM>> result = new GeneralResult<List<GoalResponseVM>>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            int getClientId = 0;

            if (!isSuperAdmin && clientId == "0")
            {
                result.Data = null;
                result.IsSuccess = false;
                result.Message = "Please enter clientID";
            }

            if (isSuperAdmin)
            {
                getClientId = (int)clientid;
            }
            else
            {
                getClientId = string.IsNullOrEmpty(clientId) == true ? 0 : Convert.ToInt32(clientId);
            }

            var res = await goalRepo.DeleteGoal(getClientId, isSuperAdmin, id);
            return Ok(res);

        }


        //[Authorize(Roles = "GetGoalById")]
        [HttpGet]
        [Route("getgoalbyid")]
        public async Task<IActionResult> GetGoalById(int clientid, int id)
        {
            IGeneralResult<List<GoalResponseVM>> result = new GeneralResult<List<GoalResponseVM>>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            int getClientId = 0;

            if (!isSuperAdmin && clientId == "0")
            {
                result.Data = null;
                result.IsSuccess = false;
                result.Message = "Please enter clientID";
            }

            if (isSuperAdmin)
            {
                getClientId = (int)clientid;
            }
            else
            {
                getClientId = string.IsNullOrEmpty(clientId) == true ? 0 : Convert.ToInt32(clientId);
            }

            var res = await goalRepo.GetGoalById(getClientId, isSuperAdmin, id);
            return Ok(res);

        }
    }
}
