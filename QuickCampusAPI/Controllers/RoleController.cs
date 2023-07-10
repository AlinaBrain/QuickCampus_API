using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class RoleController : Controller
    {
        private readonly IRoleRepo roleRepo;
        private readonly IUserRepo userRepo;
        private readonly IClientRepo clientRepo;
        private IConfiguration config;
        public RoleController(IRoleRepo roleRepo, IUserRepo userRepo, IClientRepo clientRepo, IConfiguration config)
        {
            this.roleRepo = roleRepo;
            this.userRepo = userRepo;
            this.clientRepo = clientRepo;
            this.config = config;
        }
        [HttpPost]
        [Route("roleAdd")]
        public async Task<IActionResult> roleAdd([FromBody] RoleModel vm)
        {
            IGeneralResult<RoleVm> result = new GeneralResult<RoleVm>();
            var _jwtSecretKey = config["Jwt:Key"];
            if (roleRepo.Any(x => x.Name == vm.RoleName))
            {
                result.Message = "RoleName Already Registerd!";
            }
            else
            {

                if (ModelState.IsValid)
                {
                    var user = await userRepo.GetById(vm.userId);
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

                                };
                                await roleRepo.Add(roleVm.toRoleDBModel());
                                result.Message = "Role added successfully";
                                result.IsSuccess = true;
                                result.Data = roleVm;
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

                            };
                            await roleRepo.Add(roleVm.toRoleDBModel());
                            result.Message = "Role added successfully";
                            result.IsSuccess = true;
                            result.Data = roleVm;
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

        [HttpGet]
        [Route("roleList")]
        public async Task<IActionResult> roleList()
        {
            var _jwtSecretKey = config["Jwt:Key"];
            var clientId = JwtHelper.GetUserIdFromToken(Request.Headers["Authorization"], _jwtSecretKey); 
            List<RoleVm> roleVm = new List<RoleVm>();
            var rolelist = (await roleRepo.GetAll()).ToList();
            roleVm = rolelist.Select(x => ((RoleVm)x)).ToList();
            return Ok(roleVm);
        }

        [HttpPost]
        [Route("roleEdit")]
        public async Task<IActionResult> Edit(int roleId, RoleModel vm)
        {
            IGeneralResult<RoleVm> result = new GeneralResult<RoleVm>();
            var _jwtSecretKey = config["Jwt:Key"];
            if (roleRepo.Any(x => x.Name == vm.RoleName && x.Id != roleId))
            {
                result.Message = "RoleName Already Registerd!";
            }
            else
            {
                var uId = await userRepo.GetById(vm.userId);
                var check = (uId != null && uId.IsDelete == true) ? uId : null;
                if (uId != null)
                {
                    var res = await roleRepo.GetById(roleId);
                    var clientId = JwtHelper.GetUserIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                    //var clientId = vm.ClientId.HasValue ? await clientRepo.GetById((int)vm.ClientId) : null;

                    if (clientId != null || clientId == "")
                    {
                        res.Id = roleId;
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
                            res.ModifiedBy = vm.userId;
                            res.ModofiedDate = DateTime.Now;
                            await roleRepo.Update(res);
                            result.Message = "Role data is updated successfully";
                            result.IsSuccess = true;
                            result.Data = (RoleVm)res;
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
                }
                else
                {
                    result.Message = "User Id is not valid.";
                }
                return Ok(result);

            }
            return Ok(result);
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
