using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepo departmentRepo;
        private IConfiguration _config;
        public DepartmentController(IDepartmentRepo departmentRepo, IConfiguration config)
        {
            this.departmentRepo = departmentRepo;
            _config = config;
        }

        //[Authorize(Roles = "ManageDepartment")]
        [HttpGet]
        [Route("managedepartment")]

        public async Task<IActionResult> ManageDepartment(int clientid, int pageStart = 0, int pageSize = 10)
        {
            IGeneralResult<List<DepartmentVM>> result = new GeneralResult<List<DepartmentVM>>();
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

            var res = await departmentRepo.GetAllDepartments(getClientId, isSuperAdmin, pageStart, pageSize);

            return Ok(res);
        }



        //[Authorize(Roles = "AddDepartment")]
        [HttpPost]
        [Route("adddepartment")]
        public async Task<IActionResult> AddOrUpdateDepartment(int clientid,[FromBody] DepartmentVM vm)
        {
            IGeneralResult<List<DepartmentVM>> result = new GeneralResult<List<DepartmentVM>>();
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
                var res = await departmentRepo.AddDepartments(clientid, Convert.ToInt32(userId), vm);
                return Ok(res);
            }
            else
            {
                var res = await departmentRepo.UpdateDepartments(getClientId, Convert.ToInt32(userId), vm);
                return Ok(res);
            }
        }

        //[Authorize(Roles = "ActiveInactiveDepartments")]
        [HttpPost]
        [Route("activeinactivedepartments")]
        public async Task<IActionResult> ActiveInactiveDepartments(int clientid, int id, bool status)
        {
            IGeneralResult<List<DepartmentVM>> result = new GeneralResult<List<DepartmentVM>>();
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

            var res = await departmentRepo.ActiveInactiveDepartments(getClientId, isSuperAdmin, id, status);
            return Ok(res);

        }


        //[Authorize(Roles = "DeleteDepartments")]
        [HttpPost]
        [Route("deletedepartments")]
        public async Task<IActionResult> DeleteDepartments(int clientid, int id)
        {
            IGeneralResult<List<DepartmentVM>> result = new GeneralResult<List<DepartmentVM>>();
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

            var res = await departmentRepo.DeleteDepartments(getClientId, isSuperAdmin, id);
            return Ok(res);

        }


        //[Authorize(Roles = "GetDepartmentsById")]
        [HttpGet]
        [Route("getdepartmentsbyid")]
        public async Task<IActionResult> GetDepartmentsById(int clientid, int id)
        {
            IGeneralResult<List<DepartmentVM>> result = new GeneralResult<List<DepartmentVM>>();
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

            var res = await departmentRepo.GetDepartmentsById(getClientId, isSuperAdmin, id);
            return Ok(res);

        }
    }
}
