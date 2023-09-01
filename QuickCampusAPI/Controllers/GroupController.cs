using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupRepo _grouprepo;
        private readonly IConfiguration _config;

        public GroupController( IGroupRepo groupRepo,IConfiguration configuration)
        {
            _grouprepo=groupRepo;
            _config = configuration;
        }

        [HttpGet]
        [Route("GetAllGroup")]
        public async Task<IActionResult> GetAllGroup(int clientid)
        {
            IGeneralResult<List<GroupVm>> result = new GeneralResult<List<GroupVm>>();
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
            List<Groupdl> grouplist = new List<Groupdl>();
            var cityTotalCount = 0;
            try
            {
                if (isSuperAdmin)
                {
                    cityTotalCount = (await _grouprepo.GetAll()).Where(x =>  (cid == 0 ? true : x.ClentId == cid)).Count();
                    grouplist = (await _grouprepo.GetAll()).Where(x => (cid == 0 ? true : x.ClentId == cid)).ToList();
                }
                else
                {
                    cityTotalCount = (await _grouprepo.GetAll()).Where(x => (cid == 0 ? true : x.ClentId == cid)).Count();
                    grouplist = (await _grouprepo.GetAll()).Where(x => (cid == 0 ? true : x.ClentId == cid)).ToList();
                }
                var response = grouplist.Select(x => (GroupVm)x).ToList();
                if (grouplist.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Group get successfully";
                    result.Data = response;
                    result.TotalRecordCount = grouplist.Count;
                }
                else
                {
                    result.IsSuccess = true;
                    result.Message = "Group list not found!";
                    result.Data = null;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }

    }
}
