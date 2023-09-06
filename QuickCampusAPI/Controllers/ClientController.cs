using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepo _clientRepo;
        private readonly IUserRepo _userRepo;
        private IConfiguration _config;
        private readonly IUserRoleRepo _roleRepo;
        public ClientController(IClientRepo clientRepo, IConfiguration config, IUserRepo userRepo, IUserRoleRepo userRoleRepo)
        {
            _clientRepo = clientRepo;
            _config = config;
            _userRepo = userRepo;
            _roleRepo = userRoleRepo;
        }

        [Authorize(Roles = "AddClient")]
        [HttpPost]
        [Route("AddClient")]
        public async Task<IActionResult> AddClient([FromBody] ClientVM vm)
        {
            IGeneralResult<ClientResponseVm> result = new GeneralResult<ClientResponseVm>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (_clientRepo.Any(x => x.Email == vm.Email && x.IsActive == true))
            {
                result.Message = "Email Already Registered!";
                return Ok(result);
            }
            else if (_clientRepo.Any(x => x.IsActive == true && x.Name == vm.Name.Trim()))
            {
                result.Message = "Name Already Exist!";
                return Ok(result);
            }
            else if (_clientRepo.Any(x => x.UserName == vm.UserName && x.IsActive == true))
            {
                result.Message = "Username Already Registered! Please use diffrent name";
                return Ok(result);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    vm.Password = EncodePasswordToBase64(vm.Password);

                    ClientVM clientVM = new ClientVM
                    {
                        Name = vm.Name.Trim(),
                        Email = vm.Email.Trim(),
                        Phone = vm.Phone.Trim(),
                        Address = vm.Address.Trim(),
                        CraetedBy = Convert.ToInt32(userId),
                        ModifiedBy = Convert.ToInt32(userId),
                        SubscriptionPlan = vm.SubscriptionPlan.Trim(),
                        Latitude = vm.Latitude,
                        Longitude = vm.Longitude,
                        UserName = vm.UserName.Trim(),
                        Password = vm.Password.Trim(),
                    };
                    try
                    {
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

                        
                            var res = await _roleRepo.SetClientAdminRole(userdetails.Id);
                        

                        result.Data = (ClientResponseVm)clientdata;
                        result.Message = "Client added successfully";
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
                    result.Message = "something Went Wrong";
                }

            }
            return Ok(result);
        }

        [Authorize(Roles = "EditClient")]
        [HttpPost]
        [Route("EditClient")]
        public async Task<IActionResult> EditClient([FromBody] ClientUpdateRequest vm)
        {
            IGeneralResult<ClientResponseVm> result = new GeneralResult<ClientResponseVm>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var cid = clientId == ""? 0: Convert.ToInt32(clientId);
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
                    result.Message = " Client does Not Exist";
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
                        result.Data = (ClientResponseVm)await _clientRepo.Update(res);
                        result.Message = "Client updated successfully";
                        result.IsSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        result.Message = ex.Message;
                    }
                    return Ok(result);
                }else if (isSuperAdmin)
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
                        result.Data = (ClientResponseVm)await _clientRepo.Update(res);
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
        public async Task<IActionResult> GetAllClient(int clientid, string? name, string? email, string? phone, int pageStart=1,int pageSize=10)
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

                var clientListCount = (await _clientRepo.GetAll()).Where(x => x.IsDeleted != true && (cid == 0 ? true : x.Id == cid)).Count();
                //var clientList = (await _clientRepo.GetAll()).Where(x => x.IsDeleted != true && (cid == 0 ? true : x.Id == cid)).OrderByDescending(x => x.Id).Skip(newPageStart).Take(pageSize).ToList();

                var clientList = (await _clientRepo.GetAll()).Where(x => x.IsDeleted != true && x.Name.Contains(name ?? "", StringComparison.OrdinalIgnoreCase) && x.Email.Contains(email ?? "", StringComparison.OrdinalIgnoreCase) && x.Phone.Contains(phone ?? "")).OrderByDescending(x => x.Id).Skip(newPageStart).Take(pageSize).ToList();

                
                


                var res = clientList.Select(x => ((ClientResponseVm)x)).ToList();
                if (res != null && res.Count() > 0)
                {
                    result.IsSuccess = true;
                    result.Data = res;
                    result.TotalRecordCount = clientListCount;
                }
                else
                {
                    result.Message = "Client List Not Found";
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
            var res = await _clientRepo.GetById(Id);
            if (res.IsDeleted == false)
            {

                res.IsActive = false;
                res.IsDeleted = true;
                await _clientRepo.Update(res);
                result.IsSuccess = true;
                result.Message = "Client Deleted Succesfully";
            }
            else
            {
                result.Message = "Client does Not exist";
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
            var res = await _clientRepo.GetById(id);
            if (res.IsDeleted == false)
            {

                res.IsActive = isActive;
                res.IsDeleted = false;
                var data = await _clientRepo.Update(res);
                result.Data = (ClientVM)data;
                result.IsSuccess = true;
                result.Message = "Client status changed succesfully";
            }
            else
            {
                result.Message = "Client does Not exist";
            }
            return Ok(result);
        }

        [Authorize(Roles = "DetailsClient")]
        [HttpGet]
        [Route("DetailsClient")]
        public async Task<IActionResult> DetailsClient(int Id,int clientid)
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

