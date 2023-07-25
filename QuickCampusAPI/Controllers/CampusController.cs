using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;

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
        public CampusController(IConfiguration configuration, ICampusRepo campusrepo, ICountryRepo countryRepo, IStateRepo stateRepo)
        {
            _campusrepo = campusrepo;
            _country = countryRepo;
            _staterepo = stateRepo;
            _config = configuration;


        }

        [HttpGet]
        [Route("ManageCampus")]
        public async Task<IActionResult> ManageCampus(int WalkInId,int? clientid )
        {
            IGeneralResult<List<CampusGridViewModel>> result = new GeneralResult<List<CampusGridViewModel>>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin  = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);


            int getClientId = 0;

            if (isSuperAdmin)
            {
                getClientId = (int)clientid;
            }


            var rec = await _campusrepo.GetAllCampus(string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId));
            var CampusList = rec.Where(x => x.WalkInID != null).ToList();
            var res = CampusList.Select(x => ((CampusGridViewModel)x)).ToList();

            if (!res.Any())
            {
                result.IsSuccess = true;
                result.Message = "List of Campus.";
                result.Data = res;
            }
            else
            {
                result.Message = "Campus not found!";
            }

            return Ok(result);
        }


        [HttpPost]
        [Route("AddCampus")]

        public async Task<IActionResult> AddCampus()
        {
            var model = new CampusGridViewModel()
            {
                States = new List<SelectListItem>() { },
                Countries = new List<SelectListItem>() { },
                Colleges = new List<CampusWalkInModel>()
            };
            return Ok(model);
        }

        [HttpGet]
        [Route("getcampusbyid")]
        public async Task<IActionResult> getcampusbyid(int campusId)
        {
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var result = await _campusrepo.GetCampusByID(campusId, string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId));
            return Ok(result);
        }
    }


}












