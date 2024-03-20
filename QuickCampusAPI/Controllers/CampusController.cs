﻿using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CampusController : ControllerBase
    {
        private readonly ICampusRepo _campusrepo;
        private readonly ICountryRepo _country;
        private readonly IStateRepo _staterepo;
        private IConfiguration _config;
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        public CampusController(IConfiguration configuration, ICampusRepo campusrepo, ICountryRepo countryRepo, IStateRepo stateRepo, IUserAppRoleRepo userAppRoleRepo)
        {
            _campusrepo = campusrepo;
            _country = countryRepo;
            _staterepo = stateRepo;
            _config = configuration;
            _userAppRoleRepo = userAppRoleRepo;
        }

        [Authorize(Roles = "ManageCampus")]
        [HttpGet]
        [Route("ManageCampus")]

        public async Task<IActionResult> ManageCampus(int clientid, string? search, int pageStart = 1, int pageSize = 10)
        {
            IGeneralResult<List<CampusGridViewModel>> result = new GeneralResult<List<CampusGridViewModel>>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            int getClientId = 0;
            var newPageStart = 0;
            if (pageStart > 0)
            {
                var startPage = 1;
                newPageStart = (pageStart - startPage) * pageSize;
            }
            if (!isSuperAdmin && clientId == "0")
            {
                result.Data = null;
                result.IsSuccess = false;
                result.Message = "Please enter clientID";
            }

            if (isSuperAdmin)
            {
                getClientId = (int)clientid;
            }
            else
            {
                getClientId = string.IsNullOrEmpty(clientId) == true ? 0 : Convert.ToInt32(clientId);
            }

            var rec = await _campusrepo.GetAllCampus(getClientId, isSuperAdmin, newPageStart, pageSize);
            var CampusListCount = (await _campusrepo.GetAll()).Where(x => x.IsActive == true && (getClientId == 0 ? true : x.ClientId == getClientId)).Count();
            //var CampusList = rec.Where(x => x.WalkInID != null).OrderByDescending(x=>x.WalkInID).ToList();
            var CampusList = rec.Where(x => x.IsActive == true && x.Title.Contains(search ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
            var res = CampusList.Select(x => ((CampusGridViewModel)x)).OrderByDescending(x => x.WalkInID).ToList();

            if (res.Any())
            {
                result.IsSuccess = true;
                result.Message = "List of Campus.";
                result.Data = res;
                result.TotalRecordCount = CampusListCount;
            }
            else
            {
                result.Message = "Campus not found!";
            }
            return Ok(result);
        }

        //[Authorize(Roles = "AddCampus")]
        [HttpPost]
        [Route("AddCampus")]

        public async Task<IActionResult> AddCampus(CampusGridRequestVM dto)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            try
            {
                var _jwtSecretKey = _config["Jwt:Key"];
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    LoggedInUserClientId = LoggedInUserId;
                }
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                 result = await _campusrepo.AddCampus(dto);
                

            }
            catch(Exception ex)
            {
                result.Message= "Server Error " + ex.Message; 
            }
            return Ok(result);
          }

        [Authorize(Roles = "UpdateCampus")]
        [HttpPost]
        [Route("UpdateCampus")]
        public async Task<IActionResult> UpdateCampus(CampusGridRequestVM dto, int clientid)
        {
            int clientId = 0;
            var _jwtSecretKey = _config["Jwt:Key"];
            var cId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);

            if (isSuperAdmin)
            {
                clientId = clientid;
            }
            else
            {
                clientId = string.IsNullOrEmpty(cId) ? 0 : Convert.ToInt32(cId);
            }
            var response = await _campusrepo.UpdateCampus(dto, clientId, string.IsNullOrEmpty(userId) ? 0 : Convert.ToInt32(userId));
            return Ok(response);
        }


        [Authorize(Roles = "GetCampusByCampusId")]
        [HttpGet]
        [Route("getCampusByCampusId")]
        public async Task<IActionResult> getcampusbyid(int campusId, int clientid)
        {
            IGeneralResult<CampusGridViewModel> result = new GeneralResult<CampusGridViewModel>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            int getClientId = 0;

            if (!isSuperAdmin && clientId == "0")
            {
                result.Data = null;
                result.IsSuccess = false;
                result.Message = "Please enter clientID";
            }

            if (isSuperAdmin)
            {
                getClientId = (int)clientid;
            }
            else
            {
                getClientId = string.IsNullOrEmpty(clientId) == true ? 0 : Convert.ToInt32(clientId);
            }
            var res = await _campusrepo.GetCampusByID(campusId, getClientId, isSuperAdmin);
            if (res.WalkInID > 0)
            {
                result.IsSuccess = true;
                result.Message = "Campus Data";
                result.Data = res;
            }
            return Ok(res);
        }

        [Authorize(Roles = "CampusAction")]
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


        [Authorize(Roles = "DeleteCampus")]
        [HttpDelete]
        [Route("DeleteCampus")]
        public async Task<IActionResult> DeleteCampus(int campusId, int clientid)
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

            var res = await _campusrepo.DeleteCampus(campusId, cid, isSuperAdmin);
            return Ok(res);
        }
    }
}