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

        public CampusController(IConfiguration configuration, ICampusRepo campusrepo, ICountryRepo countryRepo, IStateRepo stateRepo)
        {
            _campusrepo = campusrepo;
             _country = countryRepo;
            _staterepo = stateRepo;
        }

        [HttpGet]
        [Route("ManageCampus")]
        public async Task<IActionResult> ManageCampus(int WalkInId)
        {
            IGeneralResult<List<CampusGridViewModel>> result = new GeneralResult<List<CampusGridViewModel>>();

            var rec = await _campusrepo.GetAllCampus();
            var CampusList = rec.Where(x => x.WalkInID != null).ToList();
            var res = CampusList.Select(x => ((CampusGridViewModel)x)).ToList();

            if (res != null)
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
    }

    
}
    











