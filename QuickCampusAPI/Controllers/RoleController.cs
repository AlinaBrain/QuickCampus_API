using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly IRoleRepo roleRepo;
        private readonly IUserRepo userRepo;
        private IConfiguration config;
        public RoleController(IRoleRepo roleRepo, IUserRepo userRepo, IConfiguration config)
        {
            this.roleRepo = roleRepo;
            this.userRepo = userRepo;
            this.config = config;
        }

        [Authorize(Roles = "AddRole")]
        [HttpPost]
        [Route("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] RoleModel vm, int clientid)
        {
            IGeneralResult<RoleResponse> result = new GeneralResult<RoleResponse>();
            var _jwtSecretKey = config["Jwt:Key"];


            int cid = 0;
            var jwtSecretKey = config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
            }

            if (roleRepo.Any(x => x.Name == vm.RoleName))
            {
                result.Message = "RoleName Already Registerd!";
            }
            else
            {

                if (ModelState.IsValid)
                {
                    var userId = JwtHelper.GetuIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                    if (string.IsNullOrEmpty(userId))
                    {
                        result.Message = "User Not Found";
                        result.IsSuccess = false;
                        return Ok(result);
                    }
                    var user = await userRepo.GetById(Convert.ToInt32(userId));
                    var res = (user != null && user.IsDelete == true) ? user : null;
                    if (user != null)
                    {


                        RoleVm roleVm = new RoleVm
                        {
                            Name = vm.RoleName,
                            ClientId = cid,
                            CreatedBy = user.Id,
                            ModifiedBy = user.Id,
                            CreatedDate = DateTime.Now,
                            ModofiedDate = DateTime.Now
                        };
                        var roleData = await roleRepo.Add(roleVm.ToRoleDBModel());
                        result.Message = "Role added successfully";
                        result.IsSuccess = true;
                        result.Data = (RoleResponse)roleData;

                    }
                    else
                    {
                        result.Message = "User Id is not valid.";
                    }
                }
                else
                {
                    result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                }
                return Ok(result);
            }
            return Ok(result);
        }

        [Authorize(Roles = "GetAllRole")]
        [HttpGet]
        [Route("RoleList")]
        public async Task<IActionResult> RoleList(int clientid)
        {
            var _jwtSecretKey = config["Jwt:Key"];

            int cid = 0;
            var jwtSecretKey = config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
            }

           // List<RoleResponse> roleVm = new List<RoleResponse>();
            List<TblRole> rolelist = new List<TblRole>();
            if (isSuperAdmin)
            {
                rolelist = (await roleRepo.GetAll()).Where(x => x.IsDeleted != true && (cid == 0 ? true : x.ClientId == cid)).ToList();

            }
            else
            {
                rolelist = (await roleRepo.GetAll()).Where(x => x.IsDeleted != true && x.ClientId == cid).ToList();
            }
            return Ok(rolelist);
        }

        [Authorize(Roles = "UpdateRole")]
        [HttpPost]
        [Route("EditRole")]
        public async Task<IActionResult> EditRole(RoleModel vm, int clientid)
        {

            IGeneralResult<RoleResponse> result = new GeneralResult<RoleResponse>();

            int cid = 0;
            var jwtSecretKey = config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);

                if (cid == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "Invalid Client";
                    return Ok(result);
                }
            }
            if (roleRepo.Any(x => x.Name == vm.RoleName))
            {
                result.Message = "RoleName Already Registerd!";
                return Ok(result);
            }
            var res = await roleRepo.UpdateRole(vm, cid, isSuperAdmin);
            return Ok(res);
        }


        [Authorize(Roles = "DeleteRole")]
        [HttpDelete]
        [Route("DeleteRole")]
        public async Task<IActionResult> DeleteRole(int id, int clientid, bool isDeleted)
        {
            IGeneralResult<RoleResponse> result = new GeneralResult<RoleResponse>();

            int cid = 0;
            var jwtSecretKey = config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);

                if (cid == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "Invalid Client";
                    return Ok(result);
                }
            }
            var res = await roleRepo.DeleteRole(isDeleted, id, cid, isSuperAdmin);
            return Ok(res);
        }

        [Authorize(Roles = "ActiveRole")]
        [HttpGet]
        [Route("activeAndInactive")]
        public async Task<IActionResult> ActiveAndInactive(bool isActive, int id, int clientid)
        {
            int cid = 0;
            var jwtSecretKey = config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);

                if (cid == 0)
                {

                    IGeneralResult<RoleResponse> result = new GeneralResult<RoleResponse>();
                    result.IsSuccess = false;
                    result.Message = "Invalid Client";
                    return Ok(result);
                }
            }

            var res = await roleRepo.ActiveInActiveRole(isActive, id, cid, isSuperAdmin);
            return Ok(res);
        }


        [Authorize(Roles = "GetRole")]
        [HttpPost]
        [Route("GetRoleById")]
        public async Task<IActionResult> GetRoleBiIdRole(int rId, int clientid)
        {

            IGeneralResult<RoleResponse> result = new GeneralResult<RoleResponse>();

            int cid = 0;
            var jwtSecretKey = config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);

                if (cid == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "Invalid Client";
                    return Ok(result);
                }
            }
            var res = await roleRepo.GetRoleById(rId, cid, isSuperAdmin);
            return Ok(res);
        }



        [HttpPost]
        [Route("SetRolePermissions")]
        public async Task<IActionResult> SetRolePermissions(RoleMappingRequest roleMappingRequest)
        {
            var response = await roleRepo.SetRolePermission(roleMappingRequest);
            return Ok(response);
        }
    }
}
