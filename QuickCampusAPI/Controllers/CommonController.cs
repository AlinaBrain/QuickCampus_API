using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICountryRepo _countryRepo;
        private readonly IStateRepo _stateRepo;
        private readonly ICityRepo _cityRepo;
        private readonly IConfiguration _configuration;

        public CommonController(ICountryRepo countryRepo, IStateRepo stateRepo,
            ICityRepo cityRepo, IConfiguration configuration)
        {
            _countryRepo = countryRepo;
            _stateRepo = stateRepo;
            _cityRepo = cityRepo;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetCountries")]
        public async Task<IActionResult> GetAllCountry()
        {
            IGeneralResult<List<CountryVM>> result = new GeneralResult<List<CountryVM>>();
            try
            {
                var countryList = (await _countryRepo.GetAll()).Where(x => x.IsDeleted == false && x.IsActive == true).ToList().OrderBy(x => x.CountryName);
                var res = countryList.Select(x => ((CountryVM)x)).ToList();
                if (res != null && res.Count() > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Country fetched successfully.";
                    result.Data = res;
                    result.TotalRecordCount = res.Count();
                }
                else
                {
                    result.Message = "No record found";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetStatesByCountry")]
        public async Task<IActionResult> GetAllState(int countryId)
        {
            IGeneralResult<List<StateVM>> result = new GeneralResult<List<StateVM>>();

            List<MstCityState> stateList = new List<MstCityState>();
            try
            {
                stateList = (await _stateRepo.GetAll()).Where(x => x.IsDeleted == false && x.CountryId == countryId).ToList();
                var response = stateList.Select(x => (StateVM)x).ToList();
                if (stateList.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "States fetched successfully";
                    result.Data = response;
                    result.TotalRecordCount = response.Count();
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

        [HttpGet]
        [Route("GetAllCity")]
        public IActionResult GetAllCityByState(int stateId)
        {
            IGeneralResult<dynamic> result = new GeneralResult<dynamic>();
            List<MstCity> CityList = new List<MstCity>();
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
                    result.Message = "Cities fetched successfully.";
                    result.Data = response;
                    result.TotalRecordCount = cityTotalCount;
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
    }
}
