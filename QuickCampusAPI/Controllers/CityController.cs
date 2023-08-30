using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Linq;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepo _cityrepo;
        private readonly IConfiguration _config;

        public CityController(ICityRepo cityRepo,IConfiguration configuration)
        {
            _cityrepo = cityRepo;
            _config = configuration;
        }
        [Authorize(Roles = "GetAllCity")]
        [HttpGet]
        [Route("GetAllCity")]
        public async Task<IActionResult> GetAllCity(int clientid, int stateId, string cityName)
        {
            IGeneralResult<List<CityVm>> result = new GeneralResult<List<CityVm>>();
            var _jwtSecretKey = _config["Jwt:Key"];

            int cid = 0;
            var jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var newPageStart = 0;
            
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
            }
            List<City> citylist = new List<City>();
            var cityTotalCount = 0;
            try
            {      
                if (isSuperAdmin)
                {
                    cityTotalCount = (await _cityrepo.GetAll()).Where(x => x.IsDeleted != true && (cid == 0 ? true : x.ClientId == cid)).Count();
                    citylist = (await _cityrepo.GetAll()).Where(x => x.IsDeleted != true && (cid == 0 ? true : x.ClientId == cid) && x.StateId ==stateId ).ToList();             
                    citylist = (await _cityrepo.GetAll()).Where(x => x.IsDeleted != true && (cityName == "" ? true : x.CityName.Contains(cityName))).ToList();
                }
                else
                {
                    cityTotalCount = (await _cityrepo.GetAll()).Where(x => x.IsDeleted != true && (cid == 0 ? true : x.ClientId == cid)).Count();
                    citylist = (await _cityrepo.GetAll()).Where(x => x.IsDeleted != true && x.ClientId == cid && x.StateId==stateId).ToList();
                }
                var response = citylist.Select(x => (CityVm)x).ToList();
                if (citylist.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "City get successfully";
                    result.Data = response;
                    result.TotalRecordCount= citylist.Count;
                }
                else
                {
                    result.IsSuccess = true;
                    result.Message = "City list not found!";
                    result.Data = null;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }

        [Authorize(Roles = "GetCityById")]
        [HttpGet]
        [Route("GetCityById")]
        public async Task<IActionResult> GetCityById(int id,int clientid)
        {
            IGeneralResult<CityVm> result=new GeneralResult<CityVm>();
            
            var _jwtSecretKey = _config["Jwt:Key"];
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

            var res = await _cityrepo.GetById(id);
            if (res.IsDeleted == false && res.IsActive == true)
            {
                result.Data = (CityVm)res;
                result.IsSuccess = true;
                result.Message = "City details getting succesfully";
            }
            else
            {
                result.Message = "City does Not exist";
            }
            return Ok(result);
        }

        [Authorize(Roles = "AddCity")]
        [HttpPost]
        [Route("AddCity")]
        public async Task<IActionResult> AddCity(CityVm vm, int clientid)
        {
            IGeneralResult<CityVm> result = new GeneralResult<CityVm>();
            var _jwtSecretKey = _config["Jwt:Key"];
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
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (vm != null)
            {

                if (ModelState.IsValid)
                {
                    bool isExits = _cityrepo.Any(x => x.CityName == vm.CityName && x.IsDeleted == false);
                    if (isExits)
                    {
                        result.Message = " City is already exists";
                    }
                    else
                    {
                        {
                            CityVm city = new CityVm
                            {
                                CityName = vm.CityName,
                                StateId = vm.StateId,
                                IsActive = true,
                                IsDeleted = false,
                                ClientId = cid,
                            };
                            try
                            {
                                var citydata = await _cityrepo.Add(city.ToCityDbModel());
                                result.Data = (CityVm)citydata;
                                result.Message = "City added successfully";
                                result.IsSuccess = true;
                            }

                            catch (Exception ex)
                            {
                                result.Message = ex.Message;
                            }
                            return Ok(result);
                        }
                    }
                }
                else
                {
                    result.Message = "something Went Wrong";
                }
            }
            return Ok(result);
        }
        [Authorize(Roles = "EditCity")]
        [HttpPost]
        [Route("EditCity")]
        public async Task<IActionResult> EditCity(CityVm vm, [Optional] int clientid)
        {
            IGeneralResult<CityVm> result = new GeneralResult<CityVm>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);

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
                if (cid == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "Invalid Client";
                    return Ok(result);
                }
            }
            if (vm != null)
            {
                City city = new City();

                if (isSuperAdmin)
                {
                    city = (await _cityrepo.GetAll()).Where(w => w.IsDeleted == false && w.IsActive == true && cid == 0 ? true : w.ClientId == cid).FirstOrDefault();
                }
                else
                {
                    city = (await _cityrepo.GetAll()).Where(w => w.IsDeleted == false && w.IsActive == true && w.ClientId == cid).FirstOrDefault();
                }
                if (city == null)
                {
                    result.IsSuccess = false;
                    result.Message = " City does Not Exist";
                    return Ok(result);
                }
                bool isDeleted = (bool)city.IsDeleted ? true : false;
                if (isDeleted)
                {
                    result.IsSuccess = false;
                    result.Message = " City does Not Exist";
                    return Ok(result);
                }
                bool isExits = _cityrepo.Any(x => x.CityName == vm.CityName && x.IsDeleted == false);
                if (isExits)
                {
                    result.Message = " CityName is already exists";
                }

                {
                    if (ModelState.IsValid && vm.CityId > 0 && city.IsDeleted == false)
                    {
                        city.IsDeleted = false;
                        city.CityId = vm.CityId;
                        city.StateId = vm.StateId;
                        city.IsActive = true;
                        try
                        {
                            result.Data = (CityVm)await _cityrepo.Update(city);
                            result.Message = "City updated successfully";
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
            }
            return Ok(result);
        }
        [Authorize(Roles = "DeleteCity")]
        [HttpDelete]
        [Route("DeleteCity")]
        public async Task<IActionResult> DeleteCity(int id, int clientid, bool isDeleted)
        {
            IGeneralResult<CityVm> result = new GeneralResult<CityVm>();
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
                    result.Message = "Invalid College";
                    return Ok(result);
                }
            }
            var res = await _cityrepo.DeleteCity(isDeleted, id, cid, isSuperAdmin);
            return Ok(res);
        }

    }
}
