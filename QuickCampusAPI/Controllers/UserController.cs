using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo userRepo;
        private readonly IClientRepo clientRepo;
        private IConfiguration config;
        public UserController(IUserRepo userRepo, IClientRepo clientRepo, IConfiguration config)
        {
            this.userRepo = userRepo;
            this.clientRepo = clientRepo;
            this.config = config;
        }

        [Route("userAdd")]
        [HttpPost]
        public async Task<IActionResult> AddUser(UserModel vm)
        {
            vm.Password = EncodePasswordToBase64(vm.Password);
            IGeneralResult<UserVm> result = new GeneralResult<UserVm>();
            var _jwtSecretKey = config["Jwt:Key"];
            if (userRepo.Any(x => x.Email == vm.Email && x.IsActive == true && x.IsDelete == false))
            {
                result.Message = "Email Already Registered!";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    // Decode the JWT token and retrieve the "id" claim

                    var clientId = JwtHelper.GetUserIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);

                    //var clientId = vm.ClientId.HasValue ? await clientRepo.GetById((int)vm.ClientId) : null;

                    if (!string.IsNullOrEmpty(clientId))
                    {
                        int parsedClientId;
                        if (int.TryParse(clientId, out parsedClientId))
                        {
                            UserVm userVm = new UserVm
                            {
                                UserName = vm.Email,
                                Name = vm.Name,
                                Email = vm.Email,
                                Mobile = vm.Mobile,
                                Password = vm.Password,
                                ClientId = parsedClientId,
                                IsActive = true,
                                IsDelete = false
                            };

                            await userRepo.Add(userVm.ToUserDbModel());
                            result.IsSuccess = true;
                            result.Message = "User added successfully.";
                            result.Data = userVm;
                            return Ok(result);
                        }
                        else
                        {
                            result.Message = "Invalid Client ID format.";
                        }
                    }
                    else
                    {
                        UserVm userVm = new UserVm
                        {
                            UserName = vm.Email,
                            Name = vm.Name,
                            Email = vm.Email,
                            Mobile = vm.Mobile,
                            Password = vm.Password,
                            ClientId = 0, // Assign null to ClientId property
                            IsActive = true,
                            IsDelete = false
                        };

                        await userRepo.Add(userVm.ToUserDbModel());
                        result.IsSuccess = true;
                        result.Message = "User added successfully.";
                        result.Data = userVm;
                        return Ok(result);
                    }

                }
                else
                {
                    result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                }
            }

            return Ok(result);
        
    }

        [HttpGet]
        [Route("userList")]
        public async Task<IActionResult> userList()
        {
            List<UserVm> vm = new List<UserVm>();
            var list = (await userRepo.GetAll()).Where(x => x.IsDelete == false).ToList();
            vm = list.Select(x => ((UserVm)x)).ToList();
            return Ok(vm);
        }
        [HttpGet]
        [Route("userDelete")]
        public async Task<IActionResult> Delete(int id)
        {
            IGeneralResult<dynamic> result = new GeneralResult<dynamic>();
            var user = await userRepo.GetById(id);

            var res = (user != null&& user.IsDelete == false) ? user : null;
            if (res != null)
            {
                res.IsActive = false;
                res.IsDelete = true;
                await userRepo.Update(res);
                result.IsSuccess = true;
                result.Message = "Your data is deleted successfully";
                result.Data = res;
                return Ok(result);
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "User Id is not found.";
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("userEdit")]
        public async Task<IActionResult> Edit(int userId, UserModel vm)
        {
            IGeneralResult<UserVm> result = new GeneralResult<UserVm>();
            var _jwtSecretKey = config["Jwt:Key"];
            if (userRepo.Any(x => x.Email == vm.Email && x.IsActive == true && x.Id != userId && x.IsDelete == false))
            {
                result.Message = "Email Already Registered!";
            }
            else
            {
                var res = await userRepo.GetById(userId);
                if (res != null)
                {
                    var clientId = JwtHelper.GetUserIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                    //var clientId = vm.ClientId.HasValue ? await clientRepo.GetById((int)vm.ClientId) : null;

                    if (clientId != null || clientId == "")
                    {
                        res.Id = userId;
                        if (clientId == "")
                        {
                            res.ClientId = 0; // Assign null to ClientId property
                        }
                        else
                        {
                            res.ClientId = Convert.ToInt32(clientId);
                        }
                        res.UserName = vm.Email;
                        res.Name = vm.Name;
                        res.Email = vm.Email;
                        res.Mobile = vm.Mobile;
                        res.Password = vm.Password;
                        res.IsActive = true;
                        res.IsDelete = false;
                        await userRepo.Update(res);
                        result.Message = "User data is updated successfully";
                        result.IsSuccess = true;
                        result.Data = (UserVm)res;
                        return Ok(result);
                    }
                    else
                    {
                        result.Message = "Client ID not found.";
                    }
                }
                else
                {
                    result.Message = "User ID not found.";
                }
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("activeAndInactive")]
        public async Task<IActionResult> activeAndInactive(bool IsActive, int id)
        {
            IGeneralResult<dynamic> result = new GeneralResult<dynamic>();
            if (id > 0)
            {
                var res = await userRepo.GetById(id);
                if (res != null)
                {
                    res.IsActive = IsActive;
                    await userRepo.Update(res);
                    result.IsSuccess = true;
                    result.Message = "Your status is changed successfully";
                    result.Data = res;
                    return Ok(result);
                }
                else
                {
                    result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                }
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
