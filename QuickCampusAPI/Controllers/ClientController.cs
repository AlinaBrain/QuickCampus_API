using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using static System.Net.Mime.MediaTypeNames;

namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private readonly IRoleRepo _roleRepo;
        private readonly IClientRepo _clientRepo;
        private readonly IUserRepo _userRepo;
        private IConfiguration _config;
        private readonly IUserRoleRepo _UserRoleRepo;
        private readonly BtprojecQuickcampustestContext _context;

        public ClientController(IUserAppRoleRepo userAppRoleRepo, IRoleRepo roleRepo,
            IClientRepo clientRepo, IConfiguration config, IUserRepo userRepo, 
            IUserRoleRepo userRoleRepo, BtprojecQuickcampustestContext BtprojecQuickcampustestContext)
        {
            _userAppRoleRepo = userAppRoleRepo;
            _roleRepo = roleRepo;
            _clientRepo = clientRepo;
            _config = config;
            _userRepo = userRepo;
            _UserRoleRepo = userRoleRepo;
            _context = BtprojecQuickcampustestContext;
        }

        [Authorize(Roles = "AddClient")]
        [HttpPost]
        [Route("AddClient")]
        public async Task<IActionResult> AddClient([FromBody] ClientViewModel vm)
        {
            IGeneralResult<ClientResponseViewModel> result = new GeneralResult<ClientResponseViewModel>();
            try
            {
                if (ModelState.IsValid)
                {
                    var _jwtSecretKey = _config["Jwt:Key"];
                    var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                    if (_clientRepo.Any(x => x.Email == vm.Email && x.IsActive == true))
                    {
                        result.Message = "Email Already Registered!";
                        return Ok(result);
                    }
                    else if (_clientRepo.Any(x => x.UserName == vm.UserName && x.IsActive == true))
                    {
                        result.Message = "Username Already Registered!";
                        return Ok(result);
                    }
                    else
                    {
                        vm.Password = EncodePasswordToBase64(vm.Password);
                        ClientVM clientVM = new ClientVM
                        {
                            Name = vm.Name.Trim(),
                            Email = vm.Email.Trim(),
                            Phone = vm.Phone.Trim(),
                            Address = vm.Address.Trim(),
                            CreatedBy = Convert.ToInt32(userId),
                            ModifiedBy = Convert.ToInt32(userId),
                            SubscriptionPlan = vm.SubscriptionPlan.Trim(),
                            Latitude = vm.Latitude,
                            Longitude = vm.Longitude,
                            UserName = vm.UserName.Trim(),
                            Password = vm.Password.Trim(),
                        };

                        var clientdata = await _clientRepo.Add(clientVM.ToClientDbModel());

                        UserVm userVm = new UserVm()
                        {
                            Name = clientdata.Name,
                            Password = clientdata.Password,
                            Email = clientdata.UserName,
                            ClientId = clientdata.Id,
                            Mobile = clientdata.Phone,
                        };
                        var userdetails = await _userRepo.Add(userVm.ToUserDbModel());
                        if(userdetails.Id > 0)
                        {
                            TblUserAppRole userAppRole = new TblUserAppRole()
                            {
                                UserId = userdetails.Id,
                                RoleId = (int)common.AppRole.Client
                            };
                            var roleAdd = await _userAppRoleRepo.Add(userAppRole);
                            if(roleAdd.Id > 0)
                            {
                                bool ClientRoleCheck =  await _roleRepo.AnyAsync(x => x.Id == vm.RoleId);
                                if (ClientRoleCheck)
                                {
                                    TblUserRole userRole = new TblUserRole
                                    {
                                        RoleId = vm.RoleId,
                                        UserId = userdetails.Id
                                    };
                                    var userRoleData = await _UserRoleRepo.Add(userRole);
                                    if(userRoleData.Id > 0)
                                    {

                                        ClientResponseViewModel clientresponse = new ClientResponseViewModel
                                        {
                                            Id = clientdata.Id,
                                            Name = clientdata.Name,
                                            Email = clientdata.Email,
                                            Phone = clientdata.Phone,
                                            SubscriptionPlan = clientdata.SubscriptionPlan,
                                            Address = clientdata.Address,
                                            Latitude = clientdata.Latitude,
                                            Longitude = clientdata.Longitude,
                                            RoleName = _roleRepo.GetAllQuerable().Where(x => x.Id == vm.RoleId).Select(x => x.Name).First(),
                                            AppRoleName = ((common.AppRole)userAppRole.RoleId).ToString(),
                                            IsActive = clientdata.IsActive
                                        };
                                        result.Data = clientresponse;
                                        result.Message = "Client added successfully";
                                        result.IsSuccess = true;
                                    }
                                }
                            }
                        }
                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                }
            }

            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }
        [Authorize(Roles = "EditClient")]
        [HttpPost]
        [Route("EditClient")]
        public async Task<IActionResult> EditClient([FromBody] EditClientVm vm)
        {
            IGeneralResult<ClientResponseViewModel> result = new GeneralResult<ClientResponseViewModel>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var cid = clientId == "" ? 0 : Convert.ToInt32(clientId);
            if (_clientRepo.Any(x => x.Email == vm.Email.Trim() && x.IsDeleted != true && x.Id != vm.Id))
            {
                result.Message = "Email Already Registered!";
            }
            else
            {
                var res = await _clientRepo.GetById(vm.Id);
                bool isDeleted = (bool)res.IsDeleted ? true : false;
                if (isDeleted)
                {
                    result.Message = "Client does Not Exist";
                    return Ok(result);
                }
                var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);

                if (ModelState.IsValid && vm.Id > 0 && res.IsDeleted == false && vm.Id == cid)
                {
                    res.Email = vm.Email.Trim();
                    res.Phone = vm.Phone.Trim();
                    res.Address = vm.Address.Trim();
                    res.SubscriptionPlan = vm.SubscriptionPlan.Trim();
                    res.CraetedBy = Convert.ToInt32(userId);
                    res.ModifiedBy = Convert.ToInt32(userId);
                    res.Longitude = vm.Longitude;
                    res.Latitude = vm.Latitude;
                    res.ModofiedDate = DateTime.Now;
                    try
                    {
                        await _clientRepo.Update(res);
                        result.Message = "Client updated successfully";
                        result.IsSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        result.Message = ex.Message;
                    }
                    return Ok(result);
                }
                else if (isSuperAdmin)
                {
                    res.Email = vm.Email.Trim();
                    res.Phone = vm.Phone.Trim();
                    res.Address = vm.Address.Trim();
                    res.SubscriptionPlan = vm.SubscriptionPlan.Trim();
                    res.CraetedBy = Convert.ToInt32(userId);
                    res.ModifiedBy = Convert.ToInt32(userId);
                    res.Longitude = vm.Longitude;
                    res.Latitude = vm.Latitude;
                    res.ModofiedDate = DateTime.Now;
                    try
                    {
                         _clientRepo.Update(res);
                        result.Message = "Client updated successfully";
                        result.IsSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        result.Message = ex.Message;
                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = "You don't have a permission to update client";
                }

            }
            return Ok(result);
        }

        [Authorize(Roles = "GetAllClient")]
        [HttpGet]
        [Route("GetAllClient")]
        public async Task<IActionResult> GetAllClient(int clientid, string? search, int pageStart = 1, int pageSize = 10)
        {
            IGeneralResult<List<ClientResponseVm>> result = new GeneralResult<List<ClientResponseVm>>();
            int cid = 0;
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var newPageStart = 0;
            if (pageStart > 0)
            {
                var startPage = 1;
                newPageStart = (pageStart - startPage) * pageSize;
            }
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
            }
            try
            {
                var results = (from c in _context.TblClients
                               join u in _context.TblUsers
                               on c.CraetedBy equals u.Id

                               select new ClientResponseVm()
                               {
                                   Name = c.Name,
                                   CreatedName = u.Name,
                                   Address = c.Address,
                                   Email = c.Email,
                                   Id = c.Id,
                                   IsActive = c.IsActive,
                                   IsDeleted = c.IsDeleted,
                                   Phone = c.Phone,
                                   CreatedDate = c.CreatedDate,
                                   CraetedBy = c.CraetedBy,
                                   ModifiedName = u.Name,
                                   ModofiedDate = c.ModofiedDate,
                                   SubscriptionPlan = c.SubscriptionPlan
                               }).Where(x => (x.Name.Contains(search ?? "") || x.Email.Contains(search ?? "")) && x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();


                var clientListCount = results.Count();
                var clientList = results.Skip(newPageStart).Take(pageSize).ToList();
                var res = clientList.Select(x => ((ClientResponseVm)x)).ToList();

                if (res != null && res.Count() > 0)
                {
                    result.IsSuccess = true;
                    result.Data = res;
                }
                else
                {
                    result.Message = "No data found";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }

        [Authorize(Roles = "DeleteClient")]
        [HttpDelete]
        [Route("DeleteClient")]
        public async Task<IActionResult> DeleteClient(int Id)
        {
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            IGeneralResult<ClientVM> result = new GeneralResult<ClientVM>();
            var res = _clientRepo.GetAllQuerable().Where(x => x.Id == Id && x.IsDeleted == false).FirstOrDefault();
            //if (res.IsDeleted == false)
            if (res != null)
            {

                res.IsActive = false;
                res.IsDeleted = true;
                await _clientRepo.Update(res);
                result.IsSuccess = true;
                result.Message = "Client Deleted Successfully";
            }
            else
            {
                result.Message = "Client does not exist";
            }
            return Ok(result);
        }

        [Authorize(Roles = "ActiveInActive")]
        [HttpGet]
        [Route("activeAndInactive")]
        public async Task<IActionResult> ActiveAndInactive(bool isActive, int id)
        {
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            IGeneralResult<ClientVM> result = new GeneralResult<ClientVM>();
            var res = _clientRepo.GetAllQuerable().Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
            //if (res.IsDeleted == false)
            if (res != null)
            {

                res.IsActive = isActive;
                res.IsDeleted = false;
                var data = await _clientRepo.Update(res);
                result.Data = (ClientVM)data;
                result.IsSuccess = true;
                result.Message = "Client status changed successfully";
            }
            else
            {
                result.Message = "Client does not exist";
            }
            return Ok(result);
        }

        [Authorize(Roles = "DetailsClient")]
        [HttpGet]
        [Route("DetailsClient")]
        public async Task<IActionResult> DetailsClient(int Id, int clientid)
        {
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            IGeneralResult<ClientReponse> result = new GeneralResult<ClientReponse>();
            var res = await _clientRepo.GetById(Id);
            if (res.IsDeleted == false && res.IsActive == true)
            {
                result.Data = (ClientReponse)res;
                result.IsSuccess = true;
                result.Message = "Client details getting succesfully";
            }
            else
            {
                result.Message = "Client does Not exist";
            }
            return Ok(result);
        }

        private string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        [Authorize(Roles = "GetAllActiveClient")]
        [HttpGet]
        [Route("GetAllActiveClient")]
        public async Task<IActionResult> GetAllActiveClient(int clientid)
        {
            IGeneralResult<List<ClientResponseVm>> result = new GeneralResult<List<ClientResponseVm>>();
            int cid = 0;
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            cid = isSuperAdmin ? clientid : (string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId));
            try
            {
                var clientListCount = (await _clientRepo.GetAll()).Where(x => x.IsActive == true && (cid == 0 ? true : x.Id == cid)).Count();
                var clientList = (await _clientRepo.GetAll()).Where(x => x.IsActive == true && (cid == 0 ? true : x.Id == cid)).OrderByDescending(x => x.Id).ToList();

                var res = clientList.Select(x => ((ClientResponseVm)x)).ToList();
                if (res != null && res.Count() > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "ActiveclientList";
                    result.Data = res;
                    result.TotalRecordCount = clientListCount;
                }
                else
                {
                    result.Message = " Active Client List Not Found";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }
    }
}

