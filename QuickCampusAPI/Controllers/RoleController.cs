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
        public async Task<IActionResult> AddRole([FromBody] RoleModel vm)
        {
            IGeneralResult<RoleResponse> result = new GeneralResult<RoleResponse>();
            var _jwtSecretKey = config["Jwt:Key"];
            if (roleRepo.Any(x => x.Name == vm.RoleName))
            {
                result.Message = "RoleName Already Registerd!";
            }
            else
            {

                if (ModelState.IsValid)
                {
                    var userId = JwtHelper.GetuIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                    if(string.IsNullOrEmpty(userId))
                    {
                        result.Message = "User Not Found";
                        result.IsSuccess = false;
                        return Ok(result);
                    }
                    var user = await userRepo.GetById( Convert.ToInt32(userId));
                    var res = (user != null && user.IsDelete == true) ? user : null;
                    if (user != null)
                    {
                        var clientId = JwtHelper.GetUserIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);

                        if (!string.IsNullOrEmpty(clientId))
                        {
                            int parsedClientId;
                            if (int.TryParse(clientId, out parsedClientId))
                            {
                                RoleVm roleVm = new RoleVm
                                {
                                    Name = vm.RoleName,
                                    ClientId = parsedClientId,
                                    CreatedBy = user.Id,
                                    ModifiedBy = user.Id,
                                    CreatedDate = DateTime.Now,
                                    ModofiedDate = DateTime.Now
                                };
                             var roleData = await roleRepo.Add(roleVm.ToRoleDBModel());
                                result.Message = "Role added successfully";
                                result.IsSuccess = true;
                                result.Data = (RoleResponse)roleData;
                                return Ok(result);
                            }
                            else
                            {
                                result.Message = "Invalid Client ID format.";
                            }

                        }
                        else
                        {
                            RoleVm roleVm = new RoleVm
                            {
                                Name = vm.RoleName,
                                ClientId = null,
                                CreatedBy = user.Id,
                                ModifiedBy = user.Id,
                                CreatedDate = DateTime.Now,
                                ModofiedDate = DateTime.Now
                            };
                          var roleDataWithClientId = await roleRepo.Add(roleVm.ToRoleDBModel());
                            result.Message = "Role added successfully";
                            result.IsSuccess = true;
                            result.Data = (RoleResponse)roleDataWithClientId;
                            return Ok(result);
                        }
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
        public async Task<IActionResult> RoleList()
        {
            var _jwtSecretKey = config["Jwt:Key"];
            var  clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey); 
            List<RoleResponse> roleVm = new List<RoleResponse>();
            var rolelist = (await roleRepo.GetAll()).ToList();

            if (string.IsNullOrEmpty(clientId))
            {
                roleVm = rolelist.Select(x => (RoleResponse)x).Where(w=>w.ClientId==null).ToList();
            }
            else
            {
                roleVm = rolelist.Select(x => (RoleResponse)x).Where(w => w.ClientId == Convert.ToInt32(clientId)).ToList();
            }
            return Ok(roleVm);
        }

        [Authorize(Roles = "UpdateRole")]
        [HttpPost]
        [Route("EditRole")]
        public async Task<IActionResult> EditRole(RoleModel vm)
        {
            IGeneralResult<RoleResponse> result = new GeneralResult<RoleResponse>();
            var _jwtSecretKey = config["Jwt:Key"];
            if (roleRepo.Any(x => x.Name == vm.RoleName && x.Id != vm.Id))
            {
                result.Message = "RoleName Already Registerd!";
            }
            else
            {
                    var res = await roleRepo.GetById(vm.Id);
                    var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);

                    if (clientId != null || clientId == "")
                    {
                        res.Id = vm.Id;
                        if (clientId == "")
                        {
                            res.ClientId = null; // Assign null to ClientId property
                        }
                        else
                        {
                            res.ClientId = Convert.ToInt32(clientId);
                        }
                        if (res != null)
                        {
                            res.Name = vm.RoleName;
                            res.ModofiedDate = DateTime.Now;
                            var roleData = await roleRepo.Update(res);
                            result.Message = "Role data is updated successfully";
                            result.IsSuccess = true;
                            result.Data = (RoleResponse)roleData;
                            return Ok(result);
                        }
                        else
                        {
                            result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                        }
                    }
                    else
                    {
                        result.Message = "ClientId not found. ";
                    }        
                return Ok(result);

            }
            return Ok(result);
        }

       
        [Authorize(Roles = "DeleteRole")]
        [HttpDelete]
        [Route("DeleteRole")]
        public async Task<IActionResult> DeleteRole(int id  )
        {
            var _jwtSecretKey = config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            IGeneralResult<RoleResponse> result = new GeneralResult<RoleResponse>();
            var res = await roleRepo.GetById(id);
            if (res != null)
            {
                res.IsDeleted = true;
                res.IsActive = false;

              var userDataDeleted = await roleRepo.Update(res);
                result.IsSuccess = true;
                result.Data = (RoleResponse)userDataDeleted;
                result.Message = "Role Deleted Succesfully";
            }
            else
            {
                result.Message = "Role does Not exist";
            }
            return Ok(result);
        }

        [Authorize(Roles = "ActiveRole")]
        [HttpGet]
        [Route("activeAndInactive")]
        public async Task<IActionResult> ActiveAndInactive(bool isActive, int id)
        {
            IGeneralResult<RoleResponse> result = new GeneralResult<RoleResponse>();
            if (id > 0)
            {
                var res = await roleRepo.GetById(id);
                if (res != null && res.IsDeleted == false)
                {
                    res.IsActive = isActive;
                    await roleRepo.Update(res);
                    result.IsSuccess = true;
                    result.Message = "Your status is changed successfully";
                    result.Data = (RoleResponse)res;
                    return Ok(result);
                }
                else
                {
                    result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                }
            }
            return Ok(result);
        }

    }
}
