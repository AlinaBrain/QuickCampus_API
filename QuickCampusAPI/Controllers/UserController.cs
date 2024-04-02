using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using static QuickCampus_Core.Common.common;

namespace QuickCampusAPI.Controllers
{
    [Authorize(Roles = "Admin,Client,Client_User")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IClientRepo _clientRepo;
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private IConfiguration _config;
        private string _jwtSecretKey;
        public UserController(IUserRepo userRepo, IClientRepo clientRepo, IUserAppRoleRepo userAppRoleRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _clientRepo = clientRepo;
            _userAppRoleRepo = userAppRoleRepo;
            _config = config;
            _jwtSecretKey = _config["Jwt:Key"] ?? "";
        }

        [HttpGet]
        [Route("GetAllUser")]
        public async Task<IActionResult> UserList(string? search, int? ClientId, DataTypeFilter DataType, int pageStart = 1, int pageSize = 10)
        {
            IGeneralResult<List<UserViewVm>> result = new GeneralResult<List<UserViewVm>>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                }
                var newPageStart = 0;
                if (pageStart > 0)
                {
                    var startPage = 1;
                    newPageStart = (pageStart - startPage) * pageSize;
                }

                List<TblUser> userList = new List<TblUser>();
                List<TblUser> userData = new List<TblUser>();
                int userListCount = 0;
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    userData = _userRepo.GetAllQuerable().Where(x => (ClientId != null && ClientId > 0 ? x.ClientId == ClientId : true) && x.IsDelete == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                else
                {
                    userData = _userRepo.GetAllQuerable().Where(x => x.ClientId == Convert.ToInt32(LoggedInUserClientId) && x.IsDelete == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim();
                }
                userList = userData.Where(x => (x.Email.Contains(search ?? "", StringComparison.OrdinalIgnoreCase) || x.Name.Contains(search ?? "", StringComparison.OrdinalIgnoreCase) || x.Mobile.Contains(search ?? "", StringComparison.OrdinalIgnoreCase))).OrderBy(x => x.Name).ToList();
                userListCount = userList.Count;
                userList = userList.Skip(newPageStart).Take(pageSize).ToList();

                var response = userList.Select(x => (UserViewVm)x).ToList();

                if (userList.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Data fetched successfully.";
                    result.Data = response;
                    result.TotalRecordCount = userListCount;
                }
                else
                {
                    result.Message = "No user found!";
                }

            }
            catch (Exception ex)
            {
                result.Message = "Server error " + ex.Message;
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(UserModel vm)
        {
            IGeneralResult<UserViewVm> result = new GeneralResult<UserViewVm>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                }
                vm.Password = EncodePasswordToBase64(vm.Password ?? "");

                if (_userRepo.Any(x => x.Email == vm.Email && x.IsActive == true && x.IsDelete == false))
                {
                    result.Message = "Email already Exists";
                    return Ok(result);
                }
                if (ModelState.IsValid)
                {
                    TblUser userVm = new TblUser
                    {
                        Name = vm.Name?.Trim(),
                        Email = vm.Email?.Trim(),
                        Mobile = vm.Mobile?.Trim(),
                        Password = vm.Password.Trim(),
                        CreateDate = DateTime.Now,
                    };
                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId != (int)AppRole.Admin)
                    {
                        userVm.ClientId = Convert.ToInt32(LoggedInUserClientId);
                    }
                    else
                    {
                        userVm.ClientId = vm.ClientId;
                    }
                    var addUser = await _userRepo.Add(userVm);
                    if(addUser.Id > 0)
                    {
                        result.IsSuccess = true;
                        result.Message = "User added successfully.";
                        result.Data = (UserViewVm)addUser;
                        return Ok(result);
                    }
                    else
                    {
                        result.Message = "Something went wrong.";
                    }
                }
                else
                {
                    result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error. " + ex.Message;
            }
            return Ok(result);

        }

        [HttpPost]
        [Route("EditUser")]
        public async Task<IActionResult> EditUser(UserRequestVm vm)
        {

            IGeneralResult<UserViewVm> result = new GeneralResult<UserViewVm>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                }
                //vm.Password = EncodePasswordToBase64(vm.Password ?? "");

                if (vm.UserId != null && vm.UserId > 0)
                {
                    if (_userRepo.Any(x => x.Email == vm.Email && x.IsActive == true && x.IsDelete == false && x.Id != vm.UserId))
                    {
                        result.Message = "Email already Exists";
                        return Ok(result);
                    }
                    if (_userRepo.Any(x => x.Mobile == vm.Mobile && x.IsActive == true && x.IsDelete == false && x.Id != vm.UserId))
                    {
                        result.Message = "Mobile already Exists";
                        return Ok(result);
                    }
                    if (ModelState.IsValid)
                    {
                        TblUser user = new TblUser();

                        if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                        {
                            user = _userRepo.GetAllQuerable().Where(x => x.Id == vm.UserId && x.IsDelete == false).FirstOrDefault();
                        }
                        else
                        {
                            user = _userRepo.GetAllQuerable().Where(x => x.Id == vm.UserId && x.IsDelete == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                        }
                        if (user == null)
                        {
                            result.Message = " User does Not Exist";
                            return Ok(result);
                        }

                        user.Name = vm.Name;
                        user.Mobile = vm.Mobile;
                        user.Email = vm.Email;
                        user.ModifiedDate = DateTime.Now;

                        var updateUser = await _userRepo.Update(user);

                        result.IsSuccess = true;
                        result.Message = "User updated successfully.";
                        result.Data = (UserViewVm)updateUser;
                        return Ok(result);
                    }
                    else
                    {
                        result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                    }
                }
                else
                {
                    result.Message = "Please enter a valid User UserId.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error. " + ex.Message;
            }
            return Ok(result);
        }
       
        [HttpDelete]
        [Route("DeleteUserById")]
        public async Task<IActionResult> DeleteUser(int UserId)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                }
                if (UserId > 0)
                {
                    TblUser user = new TblUser();
                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        user = _userRepo.GetAllQuerable().Where(x => x.Id == UserId && x.IsDelete == false).FirstOrDefault();
                    }
                    else
                    {
                        user = _userRepo.GetAllQuerable().Where(x => x.Id == UserId && x.IsDelete == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (user == null)
                    {
                        result.Message = " User does Not Exist";
                        return Ok(result);
                    }
                    else
                    {
                        user.IsActive = false;
                        user.IsDelete = true;
                        user.ModifiedDate = DateTime.Now;
                        await _userRepo.Update(user);

                        result.IsSuccess = true;
                        result.Message = "User deleted successfully.";
                    }
                   
                }
                else
                {
                    result.Message = "Please enter a valid User UserId.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error! " + ex.Message;
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("UserActiveInactive")]
        public async Task<IActionResult> ActiveAndInactive(int UserId)
        {
            IGeneralResult<UserViewVm> result = new GeneralResult<UserViewVm>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                }
                if (UserId > 0)
                {
                    TblUser user = new TblUser();
                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        user = _userRepo.GetAllQuerable().Where(x => x.Id == UserId && x.IsDelete == false).FirstOrDefault();
                    }
                    else
                    {
                        user = _userRepo.GetAllQuerable().Where(x => x.Id == UserId && x.IsDelete == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (user == null)
                    {
                        result.Message = " User does Not Exist";
                        return Ok(result);
                    }
                    else
                    {
                        user.IsActive = !user.IsActive;
                        user.ModifiedDate = DateTime.Now;
                        await _userRepo.Update(user);

                        result.IsSuccess = true;
                        result.Message = "User updated successfully.";
                        result.Data = (UserViewVm)user;
                        return Ok(result);

                    }
                }
                else
                {
                    result.Message = "Please enter a valid User UserId.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error! " + ex.Message;
            }
            return Ok(result);
        }
        
        [HttpGet]
        [Route("GetUserById")]
        public async Task<IActionResult> GetUserDetailsById(int UserId)
        {
            IGeneralResult<UserViewVm> result = new GeneralResult<UserViewVm>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                }
                if (UserId > 0)
                {
                    TblUser user = new TblUser();
                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        user = _userRepo.GetAllQuerable().Where(x => x.Id == UserId && x.IsDelete == false).FirstOrDefault();
                    }
                    else
                    {
                        user = _userRepo.GetAllQuerable().Where(x => x.Id == UserId && x.IsDelete == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (user == null)
                    {
                        result.Message = " User does Not Exist";
                        return Ok(result);
                    }
                    result.IsSuccess = true;
                    result.Message = "User fetched successfully.";
                    result.Data = (UserViewVm)user;
                }
                else
                {
                    result.Message = "Please enter a valid User UserId.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error! " + ex.Message;
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
