using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
    //[Authorize]
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

        [Authorize(Roles = "ManageCampus")]
        [HttpGet]
        [Route("ManageCampus")]
       
        public async Task<IActionResult> ManageCampus(int WalkInId,int clientid=0 )
        {
            IGeneralResult<List<CampusGridViewModel>> result = new GeneralResult<List<CampusGridViewModel>>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin  = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            int getClientId = 0;
            
            if (!isSuperAdmin && clientId=="0")
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
                getClientId = Convert.ToInt32(clientId);
            }
            var rec = await _campusrepo.GetAllCampus(getClientId);
            var CampusList = rec.Where(x => x.WalkInID != null).ToList();
            var res = CampusList.Select(x => ((CampusGridViewModel)x)).ToList();

            if (res.Any())
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

        //[Authorize(Roles = "AddCampus")]
        [HttpPost]
        [Route("AddCampus")]

        public async Task<IActionResult> AddCampus(CampusGridRequestVM dto)
        {
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            
            var response = await _campusrepo.AddCampus(dto, string.IsNullOrEmpty(clientId)?0:Convert.ToInt32(clientId), string.IsNullOrEmpty(userId)?0:Convert.ToInt32(userId));

            return Ok(response);
        }

        [Authorize(Roles = "GetCampusByCampusId")]
        [HttpGet]
        [Route("getCampusByCampusId")]
        public async Task<IActionResult> getcampusbyid(int campusId)
        {
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var result = await _campusrepo.GetCampusByID(campusId, string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId));
            return Ok(result);
        }
    }


}












