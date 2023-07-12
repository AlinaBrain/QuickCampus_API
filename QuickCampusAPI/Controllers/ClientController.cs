using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]


    public class ClientController : ControllerBase
    {
        private readonly IClientRepo _clientRepo;
        private readonly IUserRepo userRepo;
        private IConfiguration config;
        public ClientController(IClientRepo clientRepo, IConfiguration config, IUserRepo userRepo)
        {
            _clientRepo = clientRepo;
            this.config = config;
            this.userRepo = userRepo;
        }

        [Authorize(Roles = "ClientList")]
        [HttpGet]
        [Route("ClientList")]
        public async Task<IActionResult> ClientList()
        {
            IGeneralResult<UserVMM> result = new GeneralResult<UserVMM>();
            List<UserVMM> vm = new List<UserVMM>();
            var _jwtSecretKey = config["Jwt:Key"];
            var cilentId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            
            if ( string.IsNullOrEmpty(cilentId))
            {
                var list = (await userRepo.GetAll()).ToList();
                vm = list.Select(x => ((UserVMM)x)).Where(w=>w.ClientId==null).ToList();
            }
            else 
            {
                int cId = Convert.ToInt32(cilentId);
                var list = (await userRepo.GetAll()).ToList();
                vm = list.Select(x => ((UserVMM)x)).Where(w=>w.ClientId== cId).ToList();
            }
            return Ok(vm);
        }

        [Authorize(Roles = "AddClient")]
        [HttpPost]
        [Route("AddClient")]
        public async Task<IActionResult> AddClient(UserVm vm)
        {
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
                    var clientId = JwtHelper.GetUserIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);

                    if (!string.IsNullOrEmpty(clientId))
                    {
                        int parsedClientId;
                        if (int.TryParse(clientId, out parsedClientId))
                        {
                            UserVm userVm = new UserVm
                            {
                                UserName = vm.UserName,
                                Name = vm.Name,
                                Email = vm.Email,
                                Mobile = vm.Mobile,
                                Password = vm.Password,
                                ClientId = parsedClientId,
                                IsActive = true,
                                IsDelete = false
                            };

                            await userRepo.Add(userVm.toUserDBModel());
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
                            UserName = vm.UserName,
                            Name = vm.Name,
                            Email = vm.Email,
                            Mobile = vm.Mobile,
                            Password = vm.Password,
                            ClientId = 0,
                            IsActive = true,
                            IsDelete = false
                        };

                        await userRepo.Add(userVm.toUserDBModel());
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

        [Authorize(Roles = "EditClient")]
        [HttpPost]
        [Route("EditClient")]
        public async Task<IActionResult> EditClient(int userId, UserEVm vm)
        {
            IGeneralResult<UserEVm> result = new GeneralResult<UserEVm>();
            var _jwtSecretKey = config["Jwt:Key"];
           
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
                        res.UserName = vm.UserName;
                        res.Name = vm.Name;
                        res.Email = vm.Email;
                        res.Mobile = vm.Mobile;
                       // res.IsActive = true;
                       // res.IsDelete = false;
                        await userRepo.Update(res);
                        result.Message = "User data is updated successfully";
                        result.IsSuccess = true;
                        result.Data = (UserEVm)res;
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
            

            return Ok(result);
        }

        [Authorize(Roles = "ActiveAndInactive")]
        [HttpGet]
        [Route("ActiveAndInactive")]
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

