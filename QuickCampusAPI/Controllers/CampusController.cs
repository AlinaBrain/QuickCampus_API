
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Net.Mail;
using System.Security.Cryptography;
using static QuickCampus_Core.Common.common;

namespace QuickCampusAPI.Controllers
{
    [Authorize(Roles = "Admin,Client,Client_User")]
    [ApiController]
    [Route("api/[controller]")]
    public class CampusController : ControllerBase
    {
        private readonly ICampusRepo _campusrepo;
        private readonly ICountryRepo _country;
        private readonly IStateRepo _staterepo;
        private IConfiguration _config;
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private readonly IUserRepo _userRepo;
        private string _jwtSecretKey;
        private ICampusWalkinCollegeRepo _campusWalkinCollegeRepo;

        public CampusController(IConfiguration configuration, ICampusRepo campusrepo, ICountryRepo countryRepo, IStateRepo stateRepo, IUserAppRoleRepo userAppRoleRepo, IUserRepo userRepo, ICampusWalkinCollegeRepo campusWalkinCollegeRepo)
        {
            _campusrepo = campusrepo;
            _country = countryRepo;
            _staterepo = stateRepo;
            _config = configuration;
            _userAppRoleRepo = userAppRoleRepo;
            _userRepo = userRepo;
            _jwtSecretKey = _config["Jwt:Key"] ?? "";
            _campusWalkinCollegeRepo = campusWalkinCollegeRepo;
        }
       
        [HttpGet]
        [Route("GetAllCampus")]

        public async Task<IActionResult> GetAllCampus(string? search, DataTypeFilter DataType, int pageStart = 1, int pageSize = 10)
        {
            IGeneralResult<List<CampusViewModel>> result = new GeneralResult<List<CampusViewModel>>();
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
                var campusTotalCount = 0;
                List<WalkIn> campusList = new List<WalkIn>();
                List<WalkIn> campusData = new List<WalkIn>();
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    campusData = _campusrepo.GetAllQuerable().Where(x => x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                else
                {
                    campusData = _campusrepo.GetAllQuerable().Where(x => x.ClientId == Convert.ToInt32(LoggedInUserClientId) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }

                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim();

                }

                campusList = campusData.Where(x => (x.Address1.Contains(search ?? "", StringComparison.OrdinalIgnoreCase) || x.Address2.Trim().Contains(search ?? "", StringComparison.OrdinalIgnoreCase))).OrderByDescending(x => x.WalkInId).ToList();

                campusTotalCount = campusList.Count;
                campusList = campusList.Skip(newPageStart).Take(pageSize).ToList();
                if (campusList.Count > 0)
                {
                    var response = campusList.Select(x => (CampusViewModel)x).ToList();
                    List<CampusViewModel> record = new List<CampusViewModel>();
                    foreach (var item in response)
                    {
                        item.CampusList = _campusWalkinCollegeRepo.GetAll(y => y.WalkInId == item.WalkInID).Result.Select(z => new CampusWalkInModel()
                        {
                            CampusId = z.CampusId,
                            StartDateTime = z.StartDateTime,
                            ExamEndTime = z.ExamEndTime.ToString(),
                            CollegeId = z.CollegeId
                        }).ToList();
                    }

                    
                        result.IsSuccess = true;
                        result.Message = "Campus get successfully";
                        result.Data = response;
                        result.TotalRecordCount = campusTotalCount;
                }
                else
                {
                    result.Message = "No Campus found!";
                }
            }
            catch (Exception ex)
            {
                result.Message = "server error! " + ex.Message;
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("AddCampus")]

        public async Task<IActionResult> AddCampus(CampusGridRequestVM dto)
        {
            IGeneralResult<CampusGridRequestVM> result = new GeneralResult<CampusGridRequestVM>();
            try
            {

                var _jwtSecretKey = _config["Jwt:Key"];
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                }
                if (ModelState.IsValid)
                {
                    var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                    result = await _campusrepo.AddOrUpdateCampus(dto);
                }
                else
                {
                    result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                }
            }

            catch (Exception ex)
            {
                result.Message = "Server Error " + ex.Message;
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("UpdateCampus")]
        public async Task<IActionResult> UpdateCampus(CampusGridRequestVM dto)
        {
            IGeneralResult<CampusGridRequestVM> result = new GeneralResult<CampusGridRequestVM>();
            try
            {
                var _jwtSecretKey = _config["Jwt:Key"];
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                }
                if (ModelState.IsValid)
                {
                    var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                    result = await _campusrepo.AddOrUpdateCampus(dto);
                }
                else
                {
                    result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server Error " + ex.Message;
            }
            return Ok(result);
        }


        [HttpGet]
        [Route("getCampusByCampusId")]
        public async Task<IActionResult> getcampusbyid(int campusId)
        {
            IGeneralResult<CampusGridViewModel> result = new GeneralResult<CampusGridViewModel>();
            try
            {
                var _jwtSecretKey = _config["Jwt:Key"];
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                }
                var res = await _campusrepo.GetCampusByID(campusId);
                if (res.Data.WalkInID > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Campus Data";
                    result.Data = res.Data;
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                result.Message = "Server Error" + ex.Message;
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("CampusActiveInActive")]
        public async Task<IActionResult> ActiveInActive(int campusId, bool status)
        {
            IGeneralResult<CampusViewModel> result = new GeneralResult<CampusViewModel>();
            try
            {
                var _jwtSecretKey = _config["Jwt:Key"];
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    var res = _campusrepo.GetAllQuerable().Where(x => x.WalkInId == campusId && x.IsDeleted == false).FirstOrDefault();
                    int campusTotalCount = 0;
                    campusTotalCount = _campusrepo.GetAllQuerable().Where(x => x.WalkInId == campusId && x.IsDeleted == false).Count();
                    if (res != null)
                    {
                        res.IsActive = !res.IsActive;
                        var data = await _campusrepo.Update(res);
                        result.Data = (CampusViewModel)data;
                        result.IsSuccess = true;
                        result.TotalRecordCount = campusTotalCount;
                        result.Message = "Campus status changed successfully";
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

        [HttpDelete]
        [Route("DeleteCampus")]
        public async Task<IActionResult> DeleteCampus(int campusId)
        {
            IGeneralResult<CampusGridViewModel> result = new GeneralResult<CampusGridViewModel>();
            try
            {
                var _jwtSecretKey = _config["Jwt:Key"];
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                }
                var res = await _campusrepo.DeleteCampus(campusId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                result.Message = "Server Error" + ex.Message;
            }
            return Ok(result);
        }
    }
}
