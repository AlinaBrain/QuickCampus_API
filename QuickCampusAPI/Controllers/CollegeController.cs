using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using Microsoft.AspNetCore.Http;
using QuickCampus_DAL.Context;
using QuickCampus_Core.Services;
using Azure;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Mail;
using System.Threading.Tasks.Dataflow;
using static QuickCampus_Core.Common.common;
using QuickCampus_Core.Common.Helper;

namespace QuickCampusAPI.Controllers
{
    [Authorize(Roles = "Admin,Client,Client_User")]
    [Route("api/[controller]")]
    [ApiController]
    public class CollegeController : ControllerBase
    {
        private readonly ICollegeRepo _collegeRepo;
        private IConfiguration _config;
        private readonly ProcessUploadFile _uploadFile;     
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly string basePath;
        private readonly ICountryRepo _countryRepo;
        private readonly IStateRepo _stateRepo;
        private readonly ICityRepo _cityRepo;
        private string baseUrl;
        private IUserRepo _userRepo;
        private readonly BtprojecQuickcampustestContext _context;
        private string _jwtSecretKey;


        public CollegeController(ICollegeRepo collegeRepo, IConfiguration config, ProcessUploadFile uploadFile,
            IUserAppRoleRepo userAppRoleRepo, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
            ICountryRepo countryRepo, IStateRepo stateRepo, ICityRepo cityRepo, BtprojecQuickcampustestContext BtprojecQuickcampustestContext, IUserRepo userRepo)
        {
            _collegeRepo = collegeRepo;
            _config = config;
            _uploadFile = uploadFile;
            _userAppRoleRepo = userAppRoleRepo;
            _hostingEnvironment = hostingEnvironment;
            _countryRepo = countryRepo;
            _stateRepo = stateRepo;
            _cityRepo = cityRepo;
            _context = BtprojecQuickcampustestContext;
            _jwtSecretKey = _config["Jwt:Key"] ?? "";
            baseUrl = _config.GetSection("APISitePath").Value;
            _userRepo = userRepo;
        }


        [HttpGet]
        [Route("GetAllCollege")]
        public async Task<IActionResult> GetAllCollege(string? search, int? ClientId, DataTypeFilter DataType = DataTypeFilter.All, int pageStart = 1, int pageSize = 10)
        {
            IGeneralResult<List<CollegeCountryStateVmmm>> result = new GeneralResult<List<CollegeCountryStateVmmm>>();
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

                List<TblCollege> collegeList = new List<TblCollege>();
                List<TblCollege> collegeData = new List<TblCollege>();
                int collegeListCount = 0;
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    collegeData = _collegeRepo.GetAllQuerable().Where(x => (ClientId != null && ClientId > 0 ? x.ClientId == ClientId : true) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                else
                {
                    collegeData = _collegeRepo.GetAllQuerable().Where(x => x.ClientId == Convert.ToInt32(LoggedInUserClientId) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim();
                }
                collegeList = collegeData.Where(x => (x.ContectPerson.Contains(search ?? "", StringComparison.OrdinalIgnoreCase) || x.CollegeName.Contains(search ?? "", StringComparison.OrdinalIgnoreCase) || x.CollegeCode.Contains(search ?? "", StringComparison.OrdinalIgnoreCase) || x.ContectEmail.Contains(search ?? "", StringComparison.OrdinalIgnoreCase) || x.ContectPhone.Contains(search ?? ""))).OrderBy(x => x.CollegeName).ToList();
                collegeListCount = collegeList.Count;
                collegeList = collegeList.Skip(newPageStart).Take(pageSize).ToList();

                var response = collegeList.Select(x => (CollegeCountryStateVmmm)x).ToList();

                if (collegeList.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Data fetched successfully.";
                    result.Data = response;
                    result.TotalRecordCount = collegeListCount;
                }
                else
                {
                    result.Message = "TblCollege list not found!";
                }

            }
            catch (Exception ex)
            {
                result.Message = "Server error " + ex.Message;
            }
            return Ok(result);
        }

        [Authorize(Roles = "ViewColleges")]
        [HttpGet]
        [Route("GetCollegeDetailsByCollegeId")]
        public async Task<IActionResult> GetCollegeDetailsById(int collegeId)
        {
            IGeneralResult<CollegeCountryStateVmmm> result = new GeneralResult<CollegeCountryStateVmmm>();
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
                if (collegeId > 0)
                {
                    TblCollege college = new TblCollege();

                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        college = _collegeRepo.GetAllQuerable().Where(x => x.CollegeId == collegeId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        college = _collegeRepo.GetAllQuerable().Where(x => x.CollegeId == collegeId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (college == null)
                    {
                        result.Message = "College does Not Exist";
                    }
                    else
                    {
                        result.IsSuccess = true;
                        result.Message = "College fetched successfully.";
                        result.Data = (CollegeCountryStateVmmm)college;
                        result.Data.Logo = Path.Combine(baseUrl, result.Data.Logo);
                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = "Please enter a valid College UserId.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error! " + ex.Message;
            }
            return Ok(result);
        }

        [Authorize(Roles ="AddColleges")]
        [HttpPost]
        [Route("AddCollege")]
        public async Task<IActionResult> AddCollege([FromForm] AddCollegeVm vm)
        {
            IGeneralResult<CollegeVM> result = new GeneralResult<CollegeVM>();
            try
            {
                if (vm == null)
                {
                    result.Message = "Your Model request in Invalid";
                    return Ok(result);
                }
                if (ModelState.IsValid)
                {
                    var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                    var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                    var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                    if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                    {
                        var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                        LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                    }
                    bool isCityExits = _cityRepo.Any(x => x.CityId == vm.CityId && x.IsActive == true && x.IsDeleted == false && x.StateId == vm.StateId);
                    if (!isCityExits)
                    {
                        result.Message = " City does not exists";
                        return Ok(result);
                    }
                    bool isStateExits = _stateRepo.Any(x => x.StateId == vm.StateId && x.IsActive == true && x.IsDeleted == false && x.CountryId == vm.CountryId);
                    if (!isStateExits)
                    {
                        result.Message = " State does not exists";
                        return Ok(result);
                    }
                    bool isCountryExits = _countryRepo.Any(x => x.CountryId == vm.CountryId && x.IsActive == true && x.IsDeleted == false);
                    if (!isCountryExits)
                    {
                        result.Message = " Country does not exists";
                        return Ok(result);
                    }
                    bool isNameExits = _collegeRepo.Any(x => x.CollegeName == vm.CollegeName && x.IsDeleted == false && (LoggedInUserRole.RoleId == (int)AppRole.Admin ? x.ClientId == vm.ClientId : x.ClientId == Convert.ToInt32(LoggedInUserClientId)));
                    if (isNameExits)
                    {
                        result.Message = "College Name is already exists";
                        return Ok(result);
                    }
                    bool isCodeExist = _collegeRepo.Any(x => x.CollegeCode == vm.CollegeCode && x.IsDeleted == false && (LoggedInUserRole.RoleId == (int)AppRole.Admin ? x.ClientId == vm.ClientId : x.ClientId == Convert.ToInt32(LoggedInUserClientId)));
                    if (isCodeExist)
                    {
                        result.Message = "College Code is already exist";
                        return Ok(result);
                    }
                    bool isContactEmailExist = _collegeRepo.Any(x => x.ContectEmail == vm.ContactEmail && x.IsDeleted == false && (LoggedInUserRole.RoleId == (int)AppRole.Admin ? x.ClientId == vm.ClientId : x.ClientId == Convert.ToInt32(LoggedInUserClientId)));
                    if (isContactEmailExist)
                    {
                        result.Message = "Contact Email is Already Exist";
                        return Ok(result);
                    }
                    bool isContactPhoneExist = _collegeRepo.Any(x => x.ContectPhone == vm.ContactPhone && x.IsDeleted == false && (LoggedInUserRole.RoleId == (int)AppRole.Admin ? x.ClientId == vm.ClientId : x.ClientId == Convert.ToInt32(LoggedInUserClientId)));
                    if (isContactPhoneExist)
                    {
                        result.Message = "Contact Phone is Already Exist";
                        return Ok(result);
                    }
                if (vm.ImagePath != null)
                {
                    var ChecKImg = _uploadFile.CheckImage(vm.ImagePath);
                    if(!ChecKImg.IsSuccess)
                    {
                        result.Message = ChecKImg.Message;
                        return Ok(result);
                    }
                }
                
                    CollegeVM college = new CollegeVM
                    {
                        CollegeName = vm.CollegeName?.Trim(),
                        Address1 = vm.Address1?.Trim(),
                        Address2 = vm.Address2?.Trim(),
                        CreatedBy = Convert.ToInt32(LoggedInUserId),
                        CityId = vm.CityId,
                        StateId = vm.StateId,
                        CountryId = vm.CountryId,
                        CollegeCode = vm.CollegeCode,
                        ContectPerson = vm.ContactPersonName?.Trim(),
                        ContectEmail = vm.ContactEmail?.Trim(),
                        ContectPhone = vm.ContactPhone?.Trim(),
                        ClientId = (LoggedInUserRole.RoleId == (int)AppRole.Admin ? vm.ClientId : Convert.ToInt32(LoggedInUserClientId))
                    };
                    var UploadLogo = _uploadFile.GetUploadFile(vm.ImagePath);
                    if (UploadLogo.IsSuccess)
                    {
                        college.Logo = UploadLogo.Data;
                        var addCollege = await _collegeRepo.Add(college.ToCollegeDbModel());
                        if (addCollege.CollegeId > 0)
                        {
                            result.IsSuccess = true;
                            result.Message = "College added successfully";
                            result.Data = (CollegeVM)addCollege;
                            result.Data.Logo = Path.Combine(baseUrl, result.Data.Logo ?? "");
                        }
                        else
                        {
                            result.Message = "Something went wrong.";
                        }
                    }
                    else
                    {
                        result.Message = UploadLogo.Message;
                        return Ok(result);
                    }
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

        [Authorize(Roles = "EditColleges")]
        [HttpPost]
        [Route("EditCollege")]
        public async Task<IActionResult> EditCollege([FromForm] AddCollegeVm vm)
        {
            IGeneralResult<AddCollegeVm> result = new GeneralResult<AddCollegeVm>();
            try
            {
                if (vm == null)
                {
                    result.Message = "Your Model request in Invalid";
                    return Ok(result);
                }
                if (ModelState.IsValid)
                {
                    var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                    var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                    var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                    if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                    {
                        var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                        LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                    }
                    if (vm.CollegeId > 0)
                    {
                        bool isCityExits = _cityRepo.Any(x => x.CityId == vm.CityId && x.IsActive == true && x.IsDeleted == false && x.StateId == vm.StateId);
                        if (!isCityExits)
                        {
                            result.Message = " City does not exists";
                            return Ok(result);
                        }
                        bool isStateExits = _stateRepo.Any(x => x.StateId == vm.StateId && x.IsActive == true && x.IsDeleted == false && x.CountryId == vm.CountryId);
                        if (!isStateExits)
                        {
                            result.Message = " State does not exists";
                            return Ok(result);
                        }
                        bool isCountryExits = _countryRepo.Any(x => x.CountryId == vm.CountryId && x.IsActive == true && x.IsDeleted == false);
                        if (!isCountryExits)
                        {
                            result.Message = " Country does not exists";
                            return Ok(result);
                        }
                        bool isCollegeNameExists = _collegeRepo.Any(x => x.CollegeName == vm.CollegeName && x.IsDeleted == false && x.CollegeId != vm.CollegeId && (LoggedInUserRole.RoleId == (int)AppRole.Admin ? x.ClientId == vm.ClientId : x.ClientId == Convert.ToInt32(LoggedInUserClientId)));
                        if (isCollegeNameExists)
                        {
                            result.Message = " College Name is already exists";
                            return Ok(result);
                        }
                        bool isCollegeCodeExist = _collegeRepo.Any(x => x.CollegeCode == vm.CollegeCode && x.IsDeleted == false && x.CollegeId != vm.CollegeId && (LoggedInUserRole.RoleId == (int)AppRole.Admin ? x.ClientId == vm.ClientId : x.ClientId == Convert.ToInt32(LoggedInUserClientId)));
                        if (isCollegeCodeExist)
                        {
                            result.Message = "College Code is already exist";
                            return Ok(result);
                        }
                        bool isContactEmailExists = _collegeRepo.Any(x => x.ContectEmail == vm.ContactEmail && x.IsDeleted == false && x.CollegeId != vm.CollegeId && (LoggedInUserRole.RoleId == (int)AppRole.Admin ? x.ClientId == vm.ClientId : x.ClientId == Convert.ToInt32(LoggedInUserClientId)));
                        if (isContactEmailExists)
                        {
                            result.Message = "Contact Email is Already Exist";
                            return Ok(result);
                        }
                        bool isContactPhoneExists = _collegeRepo.Any(x => x.ContectPhone == vm.ContactPhone && x.IsDeleted == false && x.CollegeId != vm.CollegeId && (LoggedInUserRole.RoleId == (int)AppRole.Admin ? x.ClientId == vm.ClientId : x.ClientId == Convert.ToInt32(LoggedInUserClientId)));
                        if (isContactPhoneExists)
                        {
                            result.Message = "Contact Phone is Already Exist";
                            return Ok(result);
                        }


                        TblCollege college = new TblCollege();

                        if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                        {
                            college = _collegeRepo.GetAllQuerable().Where(x => x.CollegeId == vm.CollegeId && x.IsDeleted == false).FirstOrDefault();
                        }
                        else
                        {
                            college = _collegeRepo.GetAllQuerable().Where(x => x.CollegeId == vm.CollegeId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                        }
                        if (college == null)
                        {
                            result.Message = " College does Not Exist";
                            return Ok(result);
                        }
                        else
                        {
                            college.CollegeId = vm.CollegeId ?? 0;
                            college.CollegeName = vm.CollegeName?.Trim();
                            college.Address1 = vm.Address1?.Trim();
                            college.Address2 = college.Address2?.Trim();
                            college.ModifiedBy = Convert.ToInt32(LoggedInUserId);
                            college.CityId = vm.CityId;
                            college.StateId = vm.StateId;
                            college.CountryId = vm.CountryId;
                            college.CollegeCode = vm.CollegeCode?.Trim();
                            college.ContectPerson = vm.ContactPersonName?.Trim();
                            college.ContectEmail = vm.ContactEmail?.Trim();
                            college.ContectPhone = vm.ContactPhone?.Trim();
                            college.ModifiedDate = DateTime.Now;
                            if (vm.ImagePath != null)
                            {
                                var CheckImg = _uploadFile.CheckImage(vm.ImagePath);
                                if (!CheckImg.IsSuccess)
                                {
                                    result.Message = CheckImg.Message;
                                    return Ok(result);
                                }
                            }
                            var UploadLogo = _uploadFile.GetUploadFile(vm.ImagePath);
                            if (UploadLogo.IsSuccess)
                            {
                                college.Logo = UploadLogo.Data;
                                await _collegeRepo.Update(college);
                                result.IsSuccess = true;
                                result.Message = "College Updated successfully.";
                                result.Data = vm;
                                return Ok(result);
                            }
                            else
                            {
                                result.Message = UploadLogo.Message;
                                return Ok(result);
                            }
                        }
                    }
                    else
                    {
                        result.Message = "Please enter a valid College UserId";
                    }
                }
                else
                {
                    result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                    return Ok(result);
                }
            }

            catch (Exception ex)
            {
                result.Message = "Server error " + ex.Message;
            }
            return Ok(result);
        }

        [Authorize(Roles = "DeleteColleges")]
        [HttpDelete]
        [Route("DeleteCollege")]
        public async Task<IActionResult> DeleteCollege(int CollegeId)
        {
            IGeneralResult<CollegeVM> result = new GeneralResult<CollegeVM>();
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
                if (CollegeId > 0)
                {
                    TblCollege college = new TblCollege();
                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        college = _collegeRepo.GetAllQuerable().Where(x => x.CollegeId == CollegeId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        college = _collegeRepo.GetAllQuerable().Where(x => x.CollegeId == CollegeId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (college == null)
                    {
                        result.Message = " TblCollege does Not Exist";
                    }
                    else
                    {
                        college.IsActive = false;
                        college.IsDeleted = true;
                        college.ModifiedDate = DateTime.Now;
                        college.ModifiedBy = Convert.ToInt32(LoggedInUserId);
                        await _collegeRepo.Update(college);
                        result.IsSuccess = true;
                        result.Message = "College deleted successfully.";
                        result.Data = (CollegeVM)college;
                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = "Please enter a valid College Id.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error! " + ex.Message;
            }
            return Ok(result);
        }

        [Authorize(Roles = "EditColleges")]
        [HttpGet]
        [Route("CollegeActiveInactive")]
        public async Task<IActionResult> ActiveAndInactive(int CollegeId)
        {
            IGeneralResult<CollegeVM> result = new GeneralResult<CollegeVM>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                }
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();

                if (CollegeId > 0)
                {
                    TblCollege college = new TblCollege();

                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        college = _collegeRepo.GetAllQuerable().Where(x => x.CollegeId == CollegeId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        college = _collegeRepo.GetAllQuerable().Where(x => x.CollegeId == CollegeId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (college == null)
                    {
                        result.Message = " College does Not Exist";
                        return Ok(result);
                    }
                    else
                    {
                        college.IsActive = !college.IsActive;
                        college.ModifiedDate = DateTime.Now;
                        college.ModifiedBy = Convert.ToInt32(LoggedInUserId);
                        await _collegeRepo.Update(college);
                        result.IsSuccess = true;
                        result.Message = "College status updated successfully.";
                        result.Data = (CollegeVM)college;
                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = "Please enter a valid College Id.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error! " + ex.Message;
            }
            return Ok(result);
        }
    }
}


