using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

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

        [Authorize(Roles = "AddUser")]
        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(UserModel vm, int clientid)
        {
            vm.Password = EncodePasswordToBase64(vm.Password);
            IGeneralResult<UserResponseVm> result = new GeneralResult<UserResponseVm>();
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

            if (userRepo.Any(x => x.Email == vm.Email && x.IsActive == true && x.IsDelete == false))
            {
                result.Message = "Email Already Registered!";
            }
            else
            {
                if (ModelState.IsValid)
                {

                    UserVm userVm = new UserVm
                    {
                        Name = vm.Name.Trim(),
                        Email = vm.Email.Trim(),
                        Mobile = vm.Mobile.Trim(),
                        Password = vm.Password.Trim(),
                        ClientId = cid == 0 ? null : cid,

                    };
                    var dataWithClientId = await userRepo.Add(userVm.ToUserDbModel());
                    result.IsSuccess = true;
                    result.Message = "User added successfully.";
                    result.Data = (UserResponseVm)dataWithClientId;
                    return Ok(result);
                }
                else
                {
                    result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                }
            }

            return Ok(result);

        }

        [Authorize(Roles = "EditRole")]
        [HttpPost]
        [Route("EditUser")]
        public async Task<IActionResult> EditUser(UserRequestVm vm, int clientid)
        {
            IGeneralResult<UserResponseVM> result = new GeneralResult<UserResponseVM>();
            int cid = 0;
            var _jwtSecretKey = config["Jwt:Key"];
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

            
            {
                TblUser res = new TblUser();
                if ( userRepo.Any(x => x.Email == vm.Email.Trim() && x.IsDelete != true && x.Id != vm.Id))
                {
                    result.Message = "User Email Already Registered!";
                    return Ok(result);
                }

                if (isSuperAdmin)
                {
                    res = (await userRepo.GetAll()).Where(w=> w.Id== vm.Id && w.IsDelete==false && w.IsActive==true && (cid==0?true:w.ClientId==cid)).FirstOrDefault();
                }
                else
                {
                    res = (await userRepo.GetAll()).Where(w => w.Id == vm.Id && w.IsDelete == false && w.IsActive == true && w.ClientId == cid).FirstOrDefault();
                }
                bool isDeleted = (bool)res.IsDelete ? true : false;
                if (isDeleted)
                {
                    result.Message = " User does Not Exist";
                    return Ok(result);
                }

                if (ModelState.IsValid && vm.Id > 0)
                {
                    res.Email = vm.Email.Trim();
                    res.Mobile = vm.Mobile.Trim();
                    try
                    {
                        result.Data = (UserResponseVM)await userRepo.Update(res);
                        result.Message = "User updated successfully";
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

        [Authorize(Roles = "UserList")]
        [HttpGet]
        [Route("UserList")]
        public async Task<IActionResult> UserList(int clientid, string name, int pageStart=1, int pageSize=10)
        {

            IGeneralResult<List<UserResponseVm>> result = new GeneralResult<List<UserResponseVm>>();
            var _jwtSecretKey = config["Jwt:Key"];
            var newPageStart = 0;
            if (pageStart > 0)
            {
                var startPage = 1;
                newPageStart = (pageStart - startPage) * pageSize;
            }
          

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
            List<TblUser> collegeList = new List<TblUser>();
            try
            {
                var clientListCount = 0;
                if (isSuperAdmin)
                {
                    clientListCount = (await userRepo.GetAll()).Where(x => x.IsDelete != true && (cid == 0 ? true : x.Id == cid)).Count();
                    collegeList = (await userRepo.GetAll()).Where(x => x.IsDelete == false && (cid == 0 ? true : x.ClientId == cid)).OrderByDescending(o => o.Id).Skip(newPageStart).Take(pageSize).ToList();
                    collegeList = (await userRepo.GetAll()).Where(x => x.IsDelete == false && x.Name.Contains(name)).OrderByDescending(o => o.Id).Skip(newPageStart).Take(pageSize).ToList();
                }
                else
                {
                    collegeList = (await userRepo.GetAll()).Where(x => x.IsDelete != true && x.ClientId == cid).OrderByDescending(o => o.Id).Skip(newPageStart).Take(pageSize).ToList();
                }

                var response = collegeList.Select(x => (UserResponseVm)x).ToList();


                if (response.Count() > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "User get successfully";
                    result.Data = response;
                    result.TotalRecordCount = clientListCount;
                }
                else
                {
                    result.IsSuccess = true;
                    result.Message = "College list not found!";
                    result.Data = null;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }

        [Authorize(Roles = "DeleteUser")]
        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id, int clientid, bool isDeleted)
        {
            IGeneralResult<UserVm> result = new GeneralResult<UserVm>();
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
                    result.Message = "Invalid User";
                    return Ok(result);
                }
            }
            var res = await userRepo.DeleteRole(isDeleted, id, cid, isSuperAdmin);
            return Ok(res);
        }

        [Authorize(Roles = "activeAndInActiveUser")]
        [HttpGet]
        [Route("activeAndInactive")]
        public async Task<IActionResult> ActiveAndInactive(bool isActive, int id, int clientid)
        {
            IGeneralResult<UserResponseVm> result = new GeneralResult<UserResponseVm>();
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
                    result.Message = "Invalid User";
                    return Ok(result);
                }
            }

            var res = userRepo.ActiveInActiveRole(isActive, id, cid, isSuperAdmin);
            return Ok(res);
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
        [HttpGet]
        [Route("GetUserDetailsById")]
        public async Task<IActionResult> GetUserDetailsById(int Id, int clientid)
        {
            IGeneralResult<UserResponseVm> result = new GeneralResult<UserResponseVm>();
            var _jwtSecretKey = config["Jwt:Key"];
            int cid = 0;
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

            var res = await userRepo.GetById(Id);
            if (res.IsDelete == false && res.IsActive == true)
            {
                result.Data = (UserResponseVm)res;
                result.IsSuccess = true;
                result.Message = "User details getting succesfully";
            }
            else
            {
                result.Message = "User does Not exist";
            }
            return Ok(result);
        }

    }
}
