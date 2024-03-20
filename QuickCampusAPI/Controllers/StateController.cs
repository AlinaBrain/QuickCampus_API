using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Data;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class StateController : ControllerBase
    {
        private readonly IStateRepo _stateRepo;
        private readonly IConfiguration _config;

        public StateController(IStateRepo stateRepo, IConfiguration configuration)
        {
            _stateRepo = stateRepo;
            _config = configuration;
        }
    
        [HttpGet]
        [Route("GetStatesByCountry")]
        public async Task<IActionResult> GetAllState(int countryID)
        {
            IGeneralResult<List<StateVM>> result = new GeneralResult<List<StateVM>>();
            
            List<MstCityState> stateList = new List<MstCityState>();
            try
            {
                stateList = (await _stateRepo.GetAll()).Where(x => x.IsDeleted == false && x.CountryId == countryID).ToList();
                var response = stateList.Select(x => (StateVM)x).ToList();
                if (stateList.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "MstCity_State fetched successfully";
                    result.Data = response;
                }
                else
                {
                    result.Message = "No data found!";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }

        //[Authorize(Roles = "AddState")]
        //[HttpPost]
        //[Route("AddState")]
        //public async Task<IActionResult> AddState(StateModelVm vm)
        //{
        //    IGeneralResult<StateVM> result = new GeneralResult<StateVM>();
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
        //            string input = vm.StateName;
        //            Match m = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
        //            if (!m.Success)
        //            {
        //                result.Message = "Only alphabetic characters are allowed in the name.";
        //                return Ok(result);
        //            }
        //            bool isExits = _stateRepo.Any(x => x.StateName == vm.StateName && x.IsDeleted == false);
        //            if (isExits)
        //            {
        //                result.Message = " StateName is already exists";
        //            }
        //            else
        //            {
        //                {
        //                    StateVM state = new StateVM
        //                    {
        //                        StateName = vm.StateName,
        //                        CountryId=vm.CountryId,
        //                        IsActive = true,
        //                        IsDeleted = false,
        //                        ClientId = cid,
        //                    };
        //                    try
        //                    {
        //                        var statedata = await _stateRepo.Add(state.ToStateDbModel());
        //                        result.Data = (StateVM)statedata;
        //                        result.Message = "MstCity_State added successfully";
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
        //[Authorize(Roles = "GetStateById")]
        //[HttpGet]
        //[Route("GetStateById")]
        //public async Task<IActionResult> GetStateById(int stateid)
        //{
        //    IGeneralResult<StateVM> result = new GeneralResult<StateVM>();
        //    var _jwtSecretKey = _config["Jwt:Key"];
        //    int cid = 0;
        //    var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
        //    var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
          

        //    var res = (await _stateRepo.GetAll(x=>x.IsActive==true && x.IsDeleted==false &&x.StateId==stateid)).FirstOrDefault();
        //    if (res!=null)
        //    {
        //        result.Data = (StateVM)res;
        //        result.IsSuccess = true;
        //        result.Message = "MstCity_State details getting succesfully";
        //    }
        //    else
        //    {
        //        result.Message = "MstCity_State does Not exist";
        //    }
        //    return Ok(result);
        //}
        //[Authorize(Roles = "EditState")]
        //[HttpPost]
        //[Route("EditState")]
        //public async Task<IActionResult> EditState(StateVM vm, [Optional] int clientid)
        //{
        //    IGeneralResult<StateVM> result = new GeneralResult<StateVM>();
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
        //        MstCity_State state = new MstCity_State();

        //        if (isSuperAdmin)
        //        {
        //            state = (await _stateRepo.GetAll()).Where(w => w.IsDeleted == false && w.IsActive == true && cid == 0 ? true : w.ClientId == cid).FirstOrDefault();
        //        }
        //        else
        //        {
        //            state = (await _stateRepo.GetAll()).Where(w => w.IsDeleted == false && w.IsActive == true && w.ClientId == cid).FirstOrDefault();
        //        }
        //        if (state == null)
        //        {
        //            result.IsSuccess = false;
        //            result.Message = " MstCity_State does Not Exist";
        //            return Ok(result);
        //        }
        //        bool isDeleted = (bool)state.IsDeleted ? true : false;
        //        if (isDeleted)
        //        {
        //            result.IsSuccess = false;
        //            result.Message = " College does Not Exist";
        //            return Ok(result);
        //        }
        //        bool isExits = _stateRepo.Any(x => x.StateName == vm.StateName && x.IsDeleted == false);
        //        if (isExits)
        //        {
        //            result.Message = " StateName is already exists";
        //        }
               
        //        {
        //            if (ModelState.IsValid && vm.StateId > 0 && state.IsDeleted == false)
        //            {
        //                string pattern = @"^[a-zA-Z][a-zA-Z\s]*$";
        //                string input = vm.StateName;
        //                Match m = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
        //                if (!m.Success)
        //                {
        //                    result.Message = "Only alphabetic characters are allowed in the name.";
        //                    return Ok(result);
        //                }
        //                state.IsDeleted = false;
        //                state.StateName = vm.StateName;
        //                state.CountryId = vm.CountryId;
        //                state.IsActive = true;
        //                try
        //                {
        //                    result.Data = (StateVM)await _stateRepo.Update(state);
        //                    result.Message = "MstCity_State updated successfully";
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
        //[Authorize(Roles = "DeleteState")]
        //[HttpDelete]
        //[Route("DeleteState")]
        //public async Task<IActionResult> DeleteState(int stateid)
        //{
        //    var _jwtSecretKey = _config["Jwt:Key"];
        //    var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
        //    IGeneralResult<StateVM> result = new GeneralResult<StateVM>();
        //    var res = await _stateRepo.GetById(stateid);
        //    if (res.IsDeleted == false)
        //    {
        //        res.IsActive = false;
        //        res.IsDeleted = true;
        //        await _stateRepo.Update(res);
        //        result.IsSuccess = true;
        //        result.Message = "MstCity_State Deleted Succesfully";
        //    }
        //    else
        //    {
        //        result.Message = "MstCity_State does Not exist";
        //    }
        //    return Ok(result);
        //}
    }
}
