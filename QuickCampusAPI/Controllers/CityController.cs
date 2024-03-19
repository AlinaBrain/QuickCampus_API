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
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepo _cityRepo;
        private readonly IConfiguration _config;

        public CityController(ICityRepo cityRepo,IConfiguration configuration)
        {
            _cityRepo = cityRepo;
            _config = configuration;
        }

        [HttpGet]
        [Route("GetAllCity")]
        public IActionResult GetAllCityByState(int stateId)
        {
            IGeneralResult<dynamic> result = new GeneralResult<dynamic>();
            List<City> CityList = new List<City>();
            var cityTotalCount = 0;
            try
            {
                CityList = _cityRepo.GetAllQuerable().Where(x => x.IsActive == true && x.IsDeleted == false && x.StateId == stateId).ToList();
                cityTotalCount = CityList.Count;
                  
                var response = CityList.Select(x => new
                {
                    x.CityId, 
                    x.CityName
                }).ToList();
                if (cityTotalCount > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "City fetched successfully.";
                    result.Data = response;
                    result.TotalRecordCount= cityTotalCount;
                }
                else
                {
                    result.Message = "No city found!";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error " + ex.Message;
            }
            return Ok(result);
        }

        //[Authorize(Roles = "GetCityById")]
        //[HttpGet]
        //[Route("GetCityById")]
        //public async Task<IActionResult> GetCityById(int id)
        //{
        //    IGeneralResult<CityVm> result=new GeneralResult<CityVm>();
            
        //    var _jwtSecretKey = _config["Jwt:Key"];
        //    int cid = 0;
        //    var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
        //    var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            

        //    var res = (await _cityRepo.GetAll(x=>x.IsActive==true && x.IsDeleted==false && x.CityId== id)).FirstOrDefault();
        //    if (res != null)
        //    {
        //        result.Data = (CityVm)res;
        //        result.IsSuccess = true;
        //        result.Message = "City details getting succesfully";
        //    }
        //    else
        //    {
        //        result.Message = "City does Not exist";
        //    }
        //    return Ok(result);
        //}

        //[Authorize(Roles = "AddCity")]
        //[HttpPost]
        //[Route("AddCity")]
        //public async Task<IActionResult> AddCity(CityModel vm)
        //{
        //    IGeneralResult<CityVm> result = new GeneralResult<CityVm>();
        //    var _jwtSecretKey = _config["Jwt:Key"];
        //    int cid = 0;
        //    var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
        //    var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            
        //    var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
        //    if (vm != null)
        //    {

        //        if (ModelState.IsValid)
        //        {
        //            string pattern = @"^[a-zA-Z][a-zA-Z\s]*$";
        //            string input = vm.CityName;
        //            Match m = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
        //            if (!m.Success)
        //            {
        //                result.Message = "Only alphabetic characters are allowed in the name.";
        //                return Ok(result);
        //            }
        //            bool isExits = _cityRepo.Any(x => x.CityName == vm.CityName && x.IsDeleted == false);
        //            if (isExits)
        //            {
        //                result.Message = " City is already exists";
        //            }
        //            else
        //            {
        //                {
        //                    CityVm city = new CityVm
        //                    {
        //                        CityName = vm.CityName,
        //                        StateId = vm.StateId,
        //                        IsActive = true,
        //                        IsDeleted = false,
        //                        ClientId = cid,
        //                    };
        //                    try
        //                    {
        //                        var citydata = await _cityRepo.Add(city.ToCityDbModel());
        //                        result.Data = (CityVm)citydata;
        //                        result.Message = "City added successfully";
        //                        result.IsSuccess = true;
        //                    }

        //                    catch (Exception ex)
        //                    {
        //                        result.Message = ex.Message;
        //                    }
        //                    return Ok(result);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            result.Message = "something Went Wrong";
        //        }
        //    }
        //    return Ok(result);
        //}
        //[Authorize(Roles = "EditCity")]
        //[HttpPost]
        //[Route("EditCity")]
        //public async Task<IActionResult> EditCity(CityVm vm, [Optional] int clientid)
        //{
        //    IGeneralResult<CityVm> result = new GeneralResult<CityVm>();
        //    var _jwtSecretKey = _config["Jwt:Key"];
        //    var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);

        //    int cid = 0;
        //    var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
        //    var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
        //    if (isSuperAdmin)
        //    {
        //        cid = clientid;
        //    }
        //    else
        //    {
        //        cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
        //        if (cid == 0)
        //        {
        //            result.IsSuccess = false;
        //            result.Message = "Invalid Client";
        //            return Ok(result);
        //        }
        //    }
        //    if (vm != null)
        //    {
        //        City city = new City();

        //        if (isSuperAdmin)
        //        {
        //            city = (await _cityRepo.GetAll()).Where(w => w.IsDeleted == false && w.IsActive == true && cid == 0 ? true : w.ClientId == cid).FirstOrDefault();
        //        }
        //        else
        //        {
        //            city = (await _cityRepo.GetAll()).Where(w => w.IsDeleted == false && w.IsActive == true && w.ClientId == cid).FirstOrDefault();
        //        }
        //        if (city == null)
        //        {
        //            result.IsSuccess = false;
        //            result.Message = " City does Not Exist";
        //            return Ok(result);
        //        }
        //        bool isDeleted = (bool)city.IsDeleted ? true : false;
        //        if (isDeleted)
        //        {
        //            result.IsSuccess = false;
        //            result.Message = " City does Not Exist";
        //            return Ok(result);
        //        }
        //        bool isExits = _cityRepo.Any(x => x.CityName == vm.CityName && x.IsDeleted == false);
        //        if (isExits)
        //        {
        //            result.Message = " CityName is already exists";
        //        }

        //        {
        //            if (ModelState.IsValid && vm.CityId > 0 && city.IsDeleted == false)
        //            {
        //                city.IsDeleted = false;
        //                city.CityId = vm.CityId;
        //                city.StateId = vm.StateId;
        //                city.IsActive = true;
        //                try
        //                {
        //                    result.Data = (CityVm)await _cityRepo.Update(city);
        //                    result.Message = "City updated successfully";
        //                    result.IsSuccess = true;
        //                }
        //                catch (Exception ex)
        //                {
        //                    result.Message = ex.Message;
        //                }
        //                return Ok(result);
        //            }
        //            else
        //            {
        //                result.Message = "something Went Wrong";
        //            }
        //        }
        //    }
        //    return Ok(result);
        //}
        //[Authorize(Roles = "DeleteCity")]
        //[HttpDelete]
        //[Route("DeleteCity")]
        //public async Task<IActionResult> DeleteCity(int id, int clientid, bool isDeleted)
        //{
        //    IGeneralResult<CityVm> result = new GeneralResult<CityVm>();
        //    int cid = 0;
        //    var jwtSecretKey = _config["Jwt:Key"];
        //    var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], jwtSecretKey);
        //    var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], jwtSecretKey);
        //    if (isSuperAdmin)
        //    {
        //        cid = clientid;
        //    }
        //    else
        //    {
        //        cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);

        //        if (cid == 0)
        //        {
        //            result.IsSuccess = false;
        //            result.Message = "Invalid College";
        //            return Ok(result);
        //        }
        //    }
        //    var res = await _cityRepo.DeleteCity(isDeleted, id, cid, isSuperAdmin);
        //    return Ok(res);
        //}
    }
}
