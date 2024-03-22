using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using System.Net.Mail;
using System.Security.Cryptography;

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

        public CampusController(IConfiguration configuration, ICampusRepo campusrepo, ICountryRepo countryRepo, IStateRepo stateRepo, IUserAppRoleRepo userAppRoleRepo,IUserRepo userRepo)
        {
            _campusrepo = campusrepo;
            _country = countryRepo;
            _staterepo = stateRepo;
            _config = configuration;
            _userAppRoleRepo = userAppRoleRepo;
            _userRepo = userRepo;
        }
        [HttpGet]
        [Route("ManageCampus")]

        public async Task<IActionResult> GetAllCampus()
        {
            IGeneralResult<List<CampusGridViewModel>> result = new GeneralResult<List<CampusGridViewModel>>();
            try
            {
                var _jwtSecretKey = _config["Jwt:Key"];
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = user.ClientId.ToString();
                }
                var res = await _campusrepo.GetAllCampus();
                return Ok(res);
            }
            catch (Exception ex)
            {
                result.Message = "Server Error" + ex.Message;
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
                    LoggedInUserClientId = user.ClientId.ToString();
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
                    LoggedInUserClientId = user.ClientId.ToString();
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
                    LoggedInUserClientId = user.ClientId.ToString();
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
        [Route("UpdateCampusStaus")]
        public async Task<IActionResult> UpdateCampusStaus(int campusId, bool status, int clientid)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            int cid = 0;
            var jwtSecretKey = _config["Jwt:Key"];
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
                    result.Message = "Invalid Client";
                    return Ok(result);
                }
            }

            var res = await _campusrepo.UpdateCampusStatus(campusId, cid, status, isSuperAdmin);
            return Ok(res);
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
                    LoggedInUserClientId = user.ClientId.ToString();
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
    