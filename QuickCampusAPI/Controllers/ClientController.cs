using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using QuickCampus_Core.Common;
using QuickCampus_Core.Common.Enum;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.ComponentModel.DataAnnotations;
using static QuickCampus_Core.Common.common;
using static System.Net.Mime.MediaTypeNames;

namespace QuickCampusAPI.Controllers
{
    [Authorize(Roles = "Admin")]
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
        private readonly BtprojecQuickcampusContext _context;

        public ClientController(IUserAppRoleRepo userAppRoleRepo, IRoleRepo roleRepo,
            IClientRepo clientRepo, IConfiguration config, IUserRepo userRepo,
            IUserRoleRepo userRoleRepo, BtprojecQuickcampusContext BtprojecQuickcampusContext)
        {
            _userAppRoleRepo = userAppRoleRepo;
            _roleRepo = roleRepo;
            _clientRepo = clientRepo;
            _config = config;
            _userRepo = userRepo;
            _UserRoleRepo = userRoleRepo;
            _context = BtprojecQuickcampusContext;
        }

        [HttpPost]
        [Route("AddClient")]
        public async Task<IActionResult> AddClient([FromBody] ClientViewModel vm)
        {
            IGeneralResult<ClientResponseViewModel> result = new GeneralResult<ClientResponseViewModel>();
            try
            {
                var _jwtSecretKey = _config["Jwt:Key"];
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    if (ModelState.IsValid)
                    {
                        if (_clientRepo.Any(x => x.Email == vm.Email && x.IsActive == true))
                        {
                            result.Message = "Email Already Registered!";
                            return Ok(result);
                        }
                        else if (_clientRepo.Any(x => x.UserName == vm.Email && x.IsActive == true))
                        {
                            result.Message = "Username Already Registered!";
                            return Ok(result);
                        }
                        else
                        {
                            vm.Password = EncodePasswordToBase64(vm.Password);
                            ClientVM clientVM = new ClientVM
                            {
                                Name = vm.Name?.Trim(),
                                Email = vm.Email?.Trim(),
                                Phone = vm.Phone?.Trim(),
                                Address = vm.Address?.Trim(),
                                CreatedBy = Convert.ToInt32(LoggedInUserId),
                                CreatedDate = DateTime.Now,
                                SubscriptionPlan = vm.SubscriptionPlan?.Trim(),
                                Latitude = vm.Latitude,
                                Longitude = vm.Longitude,
                                UserName = vm.Email?.Trim(),
                                Password = vm.Password.Trim(),
                            };

                            var clientData = await _clientRepo.Add(clientVM.ToClientDbModel());

                            UserVm userVm = new UserVm()
                            {
                                Name = clientData.Name,
                                Password = clientData.Password,
                                Email = clientData.UserName,
                                ClientId = clientData.Id,
                                Mobile = clientData.Phone,
                            };
                            var userDetails = await _userRepo.Add(userVm.ToUserDbModel());
                            if (userDetails.Id > 0)
                            {
                                TblUserAppRole userAppRole = new TblUserAppRole()
                                {
                                    UserId = userDetails.Id,
                                    RoleId = (int)common.AppRole.Client
                                };
                                var roleAdd = await _userAppRoleRepo.Add(userAppRole);
                                if (roleAdd.Id > 0)
                                {
                                    bool ClientRoleCheck = await _roleRepo.AnyAsync(x => x.Id == vm.RoleId);
                                    if (ClientRoleCheck)
                                    {
                                        TblUserRole userRole = new TblUserRole
                                        {
                                            RoleId = vm.RoleId,
                                            UserId = userDetails.Id
                                        };
                                        var userRoleData = await _UserRoleRepo.Add(userRole);
                                        if (userRoleData.Id > 0)
                                        {
                                            ClientResponseViewModel clientResponse = new ClientResponseViewModel
                                            {
                                                Id = clientData.Id,
                                                Name = clientData.Name,
                                                Email = clientData.Email,
                                                Phone = clientData.Phone,
                                                SubscriptionPlan = clientData.SubscriptionPlan,
                                                Address = clientData.Address,
                                                Latitude = clientData.Latitude,
                                                Longitude = clientData.Longitude,
                                                RoleName = _roleRepo.GetAllQuerable().Where(x => x.Id == vm.RoleId).Select(x => x.Name).First(),
                                                AppRoleName = ((common.AppRole)userAppRole.RoleId).ToString(),
                                                IsActive = clientData.IsActive
                                            };
                                            result.Data = clientResponse;
                                            result.Message = "Client added successfully";
                                            result.IsSuccess = true;
                                            return Ok(result);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                    }
                }
                else
                {
                    result.Message = "Access denied";
                }
            }

            catch (Exception ex)
            {
                result.Message = "Server error " + ex.Message;
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("EditClient")]
        public async Task<IActionResult> EditClient([FromBody] EditClientVm vm)
        {
            IGeneralResult<ClientResponseViewModel> result = new GeneralResult<ClientResponseViewModel>();
            try
            {
                var _jwtSecretKey = _config["Jwt:Key"];
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    if (ModelState.IsValid)
                    {
                        if (_clientRepo.Any(x => x.Email == vm.Email.Trim() && x.IsDeleted != true && x.Id != vm.Id))
                        {
                            result.Message = "Email Already Registered!";
                        }
                        else
                        {
                            var res = (await _clientRepo.GetAll(x => x.Id == vm.Id && x.IsDeleted == false)).FirstOrDefault();
                            if (res == null)
                            {
                                result.Message = "Client does Not Exist";
                                return Ok(result);
                            }
                            res.Name = vm.Name;
                            res.Email = vm.Email?.Trim();
                            res.Phone = vm.Phone?.Trim();
                            res.Address = vm.Address?.Trim();
                            res.SubscriptionPlan = vm.SubscriptionPlan?.Trim();
                            res.Longitude = vm.Longitude;
                            res.Latitude = vm.Latitude;
                            res.ModifiedBy = Convert.ToInt32(LoggedInUserId);
                            res.ModofiedDate = DateTime.Now;
                            await _clientRepo.Update(res);
                            bool ClientRoleCheck = await _roleRepo.AnyAsync(x => x.Id == vm.RoleId);
                            if (ClientRoleCheck)
                            {
                                TblUserRole userRole = new TblUserRole
                                {
                                    RoleId = vm.RoleId,
                                    UserId = _userRepo.GetAll(x => x.ClientId == vm.Id).Result.FirstOrDefault().Id
                                };
                                var userRoleData = await _UserRoleRepo.Add(userRole);
                                result.Message = "Client updated successfully";

                                result.IsSuccess = true;
                            }
                            else
                            {
                                result.Message = "Role does not exists!";
                            }
                        }
                    }
                    else
                    {
                        result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                    }
                }
                else
                {
                    result.Message = "Access denied";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllClient")]
        public async Task<IActionResult> GetAllClient(string? search, DataTypeFilter Datatype, int pageStart = 1, int pageSize = 10)
        {
            IGeneralResult<List<ClientResponseViewModel>> result = new GeneralResult<List<ClientResponseViewModel>>();
            try
            {
                var _jwtSecretKey = _config["Jwt:Key"];
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    var newPageStart = 0;
                    if (pageStart > 0)
                    {
                        var startPage = 1;
                        newPageStart = (pageStart - startPage) * pageSize;

                        List<TblClient> ClientList = new List<TblClient>();

                        ClientList = await _clientRepo.GetAll(x => x.IsDeleted == false && (Datatype == DataTypeFilter.OnlyInActive ? x.IsActive == true : (Datatype == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)));

                        int clientTotalCount = ClientList.Count;
                        //var roleData = _roleRepo.GetAll(x => x.IsActive == true && x.IsDeleted == false).Result.FirstOrDefault();
                        //var userAppRole = _userAppRoleRepo.GetAll(x => x.UserId == x.Id).Result.FirstOrDefault();
                        //var userAppRoleId = userAppRole != null ? userAppRole.RoleId : 0;
                        List<ClientResponseViewModel> data = new List<ClientResponseViewModel>();
                        data.AddRange(ClientList.Select(x => new ClientResponseViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Address = x.Address,
                            SubscriptionPlan = x.SubscriptionPlan,
                            Email = x.Email,
                            Phone = x.Phone,
                            Latitude = x.Latitude,
                            Longitude = x.Longitude,
                            IsActive = x.IsActive,
                            RoleName = _clientRepo.GetClientRoleName(x.Id),
                            AppRoleName = _clientRepo.GetClientAppRoleName(x.Id)
                        }).ToList().Where(x => (x.Name.Contains(search ?? "") || x.Email.Contains(search ?? "") || x.Address.Contains(search ?? "") || x.Phone.Contains(search ?? ""))).OrderByDescending(x => x.Id).ToList());
                        result.Data = data;
                        result.Message = "Client Get Successfully";
                        result.TotalRecordCount = clientTotalCount;
                        return Ok(result);
                    }
                    else
                    {
                        result.Message = "Access Denied";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server Error " + ex.Message;
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteClient")]
        public async Task<IActionResult> DeleteClient(int Id)
        {
            IGeneralResult<ClientVM> result = new GeneralResult<ClientVM>();
            try
            {
                var _jwtSecretKey = _config["Jwt:Key"];
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
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
                else
                {
                    result.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server Error " + ex.Message;
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("ClientActiveInactive")]
        public async Task<IActionResult> ActiveAndInactive(int id)
        {
            IGeneralResult<ActiveInactivevm> result = new GeneralResult<ActiveInactivevm>();
            try
            {
                var _jwtSecretKey = _config["Jwt:Key"];
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    var res = _clientRepo.GetAllQuerable().Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
                    int clientTotalCount = 0;
                    clientTotalCount = _clientRepo.GetAllQuerable().Where(x => x.Id == id && x.IsDeleted == false).Count();
                    if (res != null)
                    {
                        res.IsActive = !res.IsActive;
                        var data = await _clientRepo.Update(res);
                        result.Data = (ActiveInactivevm)data;
                        result.IsSuccess = true;
                        result.TotalRecordCount = clientTotalCount;
                        result.Message = "Client status changed successfully";
                    }
                }
                else
                {
                    result.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server Error " + ex.Message;
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetIdByClient")]
        public async Task<IActionResult> GetIdByClient(int clientId)
        {
            IGeneralResult<GetClientById> result = new GeneralResult<GetClientById>();
            try
            {
                var _jwtSecretKey = _config["Jwt:Key"];
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    var res = await _clientRepo.GetById(clientId);
                    if (res.IsDeleted == false && res.IsActive == true)
                    {
                        result.Data = (GetClientById)res;
                        result.IsSuccess = true;
                        result.Message = "Client details getting succesfully";
                        return Ok(result);
                    }
                    else
                    {
                        result.Message = "No record found!";
                    }
                }
                else
                {
                    result.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server Error " + ex.Message;
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

    }
}

