using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo userRepo;
        private readonly IClientRepo clientRepo;
        public UserController(IUserRepo userRepo,IClientRepo clientRepo)
        {
            this.userRepo = userRepo;
            this.clientRepo = clientRepo;

        }
        [HttpPost]
        [Route("userAdd")]
        public async Task<IActionResult> addUser([FromBody] UserModel vm)
        {
            IGeneralResult<UserVm> result = new GeneralResult<UserVm>();
            if (userRepo.Any(x => x.Email == vm.Email && x.IsActive == true && x.IsDelete == false))
            {
                result.Message = "Email Already Registerd!";
            }

            else 
            {
                if (ModelState.IsValid)
                {
                    var clientId = await clientRepo.GetById(vm.ClientId);

                    if (clientId != null) // Check if client is found
                    {
                        UserVm userVm = new UserVm
                        {
                            UserName = vm.Email,
                            Name = vm.Name,
                            Email = vm.Email,
                            Mobile = vm.Mobile,
                            Password = vm.Password,
                            ClientId = vm.ClientId,
                            IsActive = true,
                            IsDelete = false,
                        };

                        await userRepo.Add(userVm.toUserDBModel());
                        result.IsSuccess = true;
                        result.Message = "User added successfully.";
                        result.Data = userVm;
                        return Ok(result);
                    }
                    else
                    {
                        result.Message = "Client ID not found.";
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

            var res = (user != null && user.IsActive == true) ? user : null;
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
            if (userRepo.Any(x => x.Email == vm.Email && x.IsActive == true && x.IsDelete == false))
            {
                result.Message = "Email Already Registerd!";
            }
            else
            {
            var res = await userRepo.GetById(userId);
            var clientId = await clientRepo.GetById(vm.ClientId);
                if (clientId != null)
                {
                    if (res != null )
                    {
                        res.Id = userId;
                        res.ClientId = vm.ClientId;
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
                        result.Message = "User ID not found.";
                    }
                }
                else
                {
                    result.Message = "Client ID not found.";
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

    }
}
