using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;

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
        public ClientController(IClientRepo clientRepo, IConfiguration config, IUserRepo userRepo)
        {
            _clientRepo = clientRepo;
            _config = config;
            _userRepo = userRepo;
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
            }
            else if (_clientRepo.Any(x => x.IsActive == true && x.Name == vm.Name.Trim()))
            {
                result.Message = "UserName Already Exist!";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    vm.Password = EncodePasswordToBase64(vm.Password);

                    ClientVM clientVM = new ClientVM
                    {
                        Name = vm.Name,
                        Email = vm.Email,
                        Phone = vm.Phone,
                        Address = vm.Address,
                        CraetedBy = Convert.ToInt32(userId),
                        ModifiedBy = Convert.ToInt32(userId),
                        SubscriptionPlan = vm.SubscriptionPlan,
                        Latitude = vm.Latitude,
                        Longitude = vm.Longitude,
                        UserName=vm.UserName,
                        Password=vm.Password,         
                };   
                    try
                    {
                        var clientdata= await _clientRepo.Add(clientVM.ToClientDbModel());

                        UserVm userVm = new UserVm()
                        {
                           Name= clientdata.Name,
                            Password = clientdata.Password,
                            Email = clientdata.Email,
                            ClientId=clientdata.Id,
                            Mobile=clientdata.Phone,
                            UserName=clientdata.UserName,   
                            
                        };

                        var userdetails = _userRepo.Add(userVm.ToUserDbModel());
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
            if (_clientRepo.Any(x => x.Email == vm.Email && x.IsActive == true && x.Id != vm.Id ))
            {
                result.Message = "Email Already Registered!";
            }
            else if (_clientRepo.Any(x => x.IsActive == true && x.Name == vm.Name.Trim()))
            {
                result.Message = "UserName Already Exist!";
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
                
                if (ModelState.IsValid && vm.Id > 0 && res.IsDeleted==false)
                {

                    
                    ClientVM clientVM = new ClientVM
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Email = vm.Email,
                        Phone = vm.Phone,
                        Address = vm.Address,
                        SubscriptionPlan = vm.SubscriptionPlan,
                        CraetedBy = Convert.ToInt32(userId),
                        ModifiedBy = Convert.ToInt32(userId),
                        Longitude = vm.Longitude,
                        Latitude = vm.Latitude,
                    };
                    try
                    { 
                      result.Data = (ClientResponseVm)await _clientRepo.Update(clientVM.ToUpdateDbModel());
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
                    result.Message = "something Went Wrong";
                }

            }
            return Ok(result);
        }

        [Authorize(Roles = "GetAllClient")]
        [HttpGet]
        [Route("GetAllClient")]
        public async Task<IActionResult> GetAllClient()
        {
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            IGeneralResult<List<ClientResponseVm>> result = new GeneralResult<List<ClientResponseVm>>();
            try
            {
                var categoryList = (await _clientRepo.GetAll()).Where(x => x.IsDeleted == false || x.IsDeleted == null).ToList();
                var res = categoryList.Select(x => ((ClientResponseVm)x)).ToList();
                if (res != null)
                {
                    result.IsSuccess = true;
                    result.Message = "ClientList";
                    result.Data = res;
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
            if (res.IsDeleted ==false)
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

                res.IsActive = false;
                res.IsDeleted = true;
              var data =  await _clientRepo.Update(res);
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
        public async Task<IActionResult> DetailsClient(int Id)
        {
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            IGeneralResult<ClientVM> result = new GeneralResult<ClientVM>();
            var res = await _clientRepo.GetById(Id);
            if (res.IsDeleted == false && res.IsActive == true)
            {
                result.Data = (ClientVM)res;
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
    }
}

