//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using QuickCampus_Core.Common;
//using QuickCampus_Core.Interfaces;
//using QuickCampus_Core.ViewModel;
//using QuickCampus_DAL.Context;
//using System.Text.RegularExpressions;
//using static QuickCampus_Core.Common.common;

//namespace QuickCampusAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AdminController : ControllerBase
//    {
//        private readonly IUserAppRoleRepo _userAppRoleRepo;
//        private readonly IRoleRepo _roleRepo;
//        private readonly IClientRepo _clientRepo;
//        private readonly IUserRepo _userRepo;
//        private IConfiguration _config;
//        private readonly IUserRoleRepo _UserRoleRepo;

//        public AdminController(IUserAppRoleRepo userAppRoleRepo, IRoleRepo roleRepo,
//            IClientRepo clientRepo, IConfiguration config, IUserRepo userRepo,
//            IUserRoleRepo userRoleRepo)
//        {
//            _userAppRoleRepo = userAppRoleRepo;
//            _roleRepo = roleRepo;
//            _clientRepo = clientRepo;
//            _config = config;
//            _userRepo = userRepo;
//            _UserRoleRepo = userRoleRepo;
//        }
//        [HttpPost]
//        [Route("AddAdmin")]
//        public async Task<IActionResult> AddClient([FromBody] ClientViewModel vm)
//        {
//            IGeneralResult<ClientResponseViewModel> result = new GeneralResult<ClientResponseViewModel>();
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    if (_clientRepo.Any(x => x.Email == vm.Email && x.IsDeleted == false))
//                    {
//                        result.Message = "Email Already Registered!";
//                        return Ok(result);
//                    }
//                    else if (_clientRepo.Any(x => x.Phone == vm.Phone && x.IsDeleted == false))
//                    {
//                        result.Message = "Phone Number Already Register";
//                        return Ok(result);
//                    }
//                    else
//                    {
//                        vm.Password = EncodePasswordToBase64(vm.Password);
//                        ClientVM clientVM = new ClientVM
//                        {
//                            Name = vm.Name?.Trim(),
//                            Email = vm.Email?.Trim(),
//                            Phone = vm.Phone?.Trim(),
//                            Address = vm.Address?.Trim(),
//                            CreatedBy = 0,
//                            CreatedDate = DateTime.Now,
//                            Latitude = vm.Latitude,
//                            Longitude = vm.Longitude,
//                            UserName = vm.Email?.Trim(),
//                            Password = vm.Password.Trim(),
//                        };

//                        var clientData = await _clientRepo.Add(clientVM.ToClientDbModel());

//                        UserVm userVm = new UserVm()
//                        {
//                            Name = clientData.Name,
//                            Password = clientData.Password,
//                            Email = clientData.Email,
//                            ClientId = clientData.Id,
//                            Mobile = clientData.Phone,
//                        };
//                        var userDetails = await _userRepo.Add(userVm.ToUserDbModel());
//                        if (userDetails.Id > 0)
//                        {
//                            TblUserAppRole userAppRole = new TblUserAppRole()
//                            {
//                                UserId = userDetails.Id,
//                                RoleId = (int)common.AppRole.Client
//                            };
//                            var roleAdd = await _userAppRoleRepo.Add(userAppRole);
//                            if (roleAdd.Id > 0)
//                            {
//                                var ClientRoleCheck = _roleRepo.GetAllQuerable().Where(x => x.Name == "Admin").FirstOrDefault();
//                                if (ClientRoleCheck != null)
//                                {
//                                    TblUserRole userRole = new TblUserRole
//                                    {
//                                        RoleId = ClientRoleCheck.Id,
//                                        UserId = userDetails.Id
//                                    };
//                                    var userRoleData = await _UserRoleRepo.Add(userRole);
//                                    if (userRoleData.Id > 0)
//                                    {
//                                        ClientResponseViewModel clientResponse = new ClientResponseViewModel
//                                        {
//                                            Id = clientData.Id,
//                                            Name = clientData.Name,
//                                            Email = clientData.Email,
//                                            Phone = clientData.Phone,
//                                            Address = clientData.Address,
//                                            Latitude = clientData.Latitude,
//                                            Longitude = clientData.Longitude,
//                                            RoleName = _roleRepo.GetAllQuerable().Where(x => x.Id == ClientRoleCheck.Id).Select(x => x.Name).First(),
//                                            AppRoleName = ((common.AppRole)userAppRole.RoleId).ToString(),
//                                            IsActive = clientData.IsActive
//                                        };
//                                        result.Data = clientResponse;
//                                        result.Message = "Client added successfully";
//                                        result.IsSuccess = true;
//                                        return Ok(result);
//                                    }
//                                }
//                                else
//                                {
//                                    result.Message = "Error occur, when try to add client role.";
//                                    return Ok(result);
//                                }
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
//                }
               
//            }

//            catch (Exception ex)
//            {
//                result.Message = "Server error " + ex.Message;
//            }
//            return Ok(result);
//        }

//        [HttpPost]
//        [Route("AddRole")]
//        public async Task<IActionResult> AddRole(RoleModel vm)
//        {
//            IGeneralResult<RoleViewVm> result = new GeneralResult<RoleViewVm>();
//            try
//            {
//                if (_roleRepo.Any(x => x.Name == vm.RoleName))
//                {
//                    result.Message = "Role Already exists";
//                    return Ok(result);
//                }

//                if (ModelState.IsValid)
//                {
//                    string pattern = @"^[a-zA-Z][a-zA-Z\s]*$";
//                    string role = vm.RoleName ?? "";
//                    Match m = Regex.Match(role, pattern, RegexOptions.IgnoreCase);
//                    if (!m.Success)
//                    {
//                        result.Message = "Only alphabetic characters are allowed in the name.";
//                        return Ok(result);
//                    }
//                    TblRole roleVm = new TblRole
//                    {
//                        Name = vm.RoleName,
//                        CreatedBy = 1,
//                        CreatedDate = DateTime.Now,
//                        IsActive = true,
//                        IsDeleted = false,
//                    };

//                    roleVm.ClientId = 1;
//                    var addRoleData = await _roleRepo.Add(roleVm);
//                    if (addRoleData.Id > 0)
//                    {
//                        if (vm.Permission.Count > 0)
//                        {
//                            var addPermissions = await _roleRepo.AddRolePermissions(vm.Permission, addRoleData.Id);
//                            if (!addPermissions.IsSuccess)
//                            {
//                                result.Message = addPermissions.Message;
//                                return Ok(result);
//                            }

//                        }
//                        result.Message = "Role added successfully.";
//                        result.IsSuccess = true;
//                        result.Data = (RoleViewVm)addRoleData;
//                        return Ok(result);
//                    }
//                    else
//                    {
//                        result.Message = "Something went wrong.";
//                    }
//                }
//                else
//                {
//                    result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
//                }
//                return Ok(result);
//            }
//            catch (Exception ex)
//            {
//                result.Message = "Server error " + ex.Message;
//            }
//            return Ok(result);
//        }
//        private string EncodePasswordToBase64(string password)
//        {
//            try
//            {
//                byte[] encData_byte = new byte[password.Length];
//                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
//                string encodedData = Convert.ToBase64String(encData_byte);
//                return encodedData;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Error in base64Encode" + ex.Message);
//            }
//        }
//    }
//}
