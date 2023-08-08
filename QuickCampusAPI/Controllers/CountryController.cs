using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Data;

namespace QuickCampusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepo _countryrepo;
        private readonly IConfiguration _config;

        public CountryController(ICountryRepo countryRepo, IConfiguration configuration)
        {
            _countryrepo = countryRepo;
            _config = configuration;
        }
        [Authorize(Roles = "GetAllCountry")]
        [HttpGet]
        [Route("GetAllCountry")]
        public async Task<IActionResult> GetAllCountry(int clientid)
        {
            IGeneralResult<List<CountryVM>> result = new GeneralResult<List<CountryVM>>();
            var _jwtSecretKey = _config["Jwt:Key"];

            int cid = 0;
            var jwtSecretKey = _config["Jwt:Key"];
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
            List<Country> countrylist = new List<Country>();
            try
            {
                if (isSuperAdmin)
                {
                    countrylist = (await _countryrepo.GetAll()).Where(x => x.IsDeleted != true && (cid == 0 ? true : x.ClientId == cid)).ToList();
                }
                else
                {
                    countrylist = (await _countryrepo.GetAll()).Where(x => x.IsDeleted != true && x.ClientId == cid).ToList();
                }
                var response = countrylist.Select(x => (CountryVM)x).ToList();
                if (countrylist.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Country get successfully";
                    result.Data = response;
                }
                else
                {
                    result.IsSuccess = true;
                    result.Message = "Country list not found!";
                    result.Data = null;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }
        [Authorize(Roles = "GetCountryById")]
        [HttpGet]
        [Route("GetCountryById")]
        public async Task<IActionResult> GetCountryById(int countryid,int clientid)
        {
            IGeneralResult<CountryVM> result = new GeneralResult<CountryVM>();
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

            var res = await _countryrepo.GetById(countryid);
            if (res.IsDeleted == false && res.IsActive == true)
            {
                result.Data = (CountryVM)res;
                result.IsSuccess = true;
                result.Message = "Country details getting succesfully";
            }
            else
            {
                result.Message = "Country does Not exist";
            }
            return Ok(result);
        }
        [Authorize(Roles = "AddCountry")]
        [HttpPost]
        [Route("AddCountry")]
        public async Task<IActionResult> AddCountry(CountryVM countryVM,int clientid)
        {
            IGeneralResult<CountryVM> result = new GeneralResult<CountryVM>();
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
            if (countryVM != null)
            {
                if (ModelState.IsValid)
                {
                    bool isExits = _countryrepo.Any(x => x.CountryName == countryVM.CountryName && x.IsDeleted == false);
                    if (isExits)
                    {
                        result.Message = " Country is already exists";
                    }
                    else
                    {
                        {
                            CountryVM country = new CountryVM
                            {
                                CountryName=countryVM.CountryName.Trim(),
                                IsActive=true,
                                IsDeleted=false,
                                ClientId = cid,
                            };
                            try
                            {
                                var countrydata = await _countryrepo.Add(country.ToCountryDbModel());
                                result.Data = (CountryVM)countrydata;
                                result.Message = "Country added successfully";
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
        [Authorize(Roles = "EditCountry")]
        [HttpPost]
        [Route("EditCountry")]
        public async Task<IActionResult> EditCountry(CountryVM vm,int clientid)
        {
            IGeneralResult<CountryVM> result = new GeneralResult<CountryVM>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (_countryrepo.Any(x => x.CountryName == vm.CountryName && x.IsActive == true && x.CountryId != vm.CountryId))
            {
                result.Message = "Email Already Registered!";
            }
            else
            {
                var res = await _countryrepo.GetById(vm.CountryId);
                bool isDeleted = (bool)res.IsDeleted ? true : false;
                if (isDeleted)
                {
                    result.Message = " Country does Not Exist";
                    return Ok(result);
                }

                if (ModelState.IsValid && vm.CountryId > 0 && res.IsDeleted == false)
                {
                    res.CountryName = vm.CountryName.Trim();
                    res.IsActive = true;
                    res.CountryId = vm.CountryId;
                    res.IsDeleted = false;
                   
                    try
                    {
                        result.Data = (CountryVM)await _countryrepo.Update(res);
                        result.Message = "Country updated successfully";
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
            return Ok(result);
        }
        [Authorize(Roles = "DeleteCountry")]
        [HttpDelete]
        [Route("DeleteCountry")]
        public async Task<IActionResult> DeleteCountry(int countryid)
        {
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            IGeneralResult<CountryVM> result = new GeneralResult<CountryVM>();
            var res = await _countryrepo.GetById(countryid);
            if (res.IsDeleted == false)
            {
                res.IsActive = false;
                res.IsDeleted = true;
                await _countryrepo.Update(res);
                result.IsSuccess = true;
                result.Message = "Country Deleted Succesfully";
            }
            else
            {
                result.Message = "Country does Not exist";
            }
            return Ok(result);
        }
    }
}
