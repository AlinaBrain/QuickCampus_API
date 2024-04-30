using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using System.Web.Http.Results;
using static QuickCampus_Core.Common.common;

namespace QuickCampusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IApplicantRepo _applicantRepo;
        private readonly ICollegeRepo _collegeRepo;
        private readonly IQuestion _question;
        private readonly ICampusRepo _campusRepo;
        private readonly IClientRepo _clientRepo;
        private readonly IConfiguration _config;
        private readonly IUserRepo _userRepo;
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private string _jwtSecretKey;

        public DashboardController(IApplicantRepo applicantRepo, ICollegeRepo collegeRepo, IQuestion question, ICampusRepo campusRepo, IClientRepo clientRepo, IUserRepo userRepo, IUserAppRoleRepo userAppRoleRepo, IConfiguration configuration)
        {
            _applicantRepo = applicantRepo;
            _collegeRepo = collegeRepo;
            _question = question;
            _campusRepo = campusRepo;
            _clientRepo = clientRepo;
            _config = configuration;
            _userRepo = userRepo;
            _userAppRoleRepo = userAppRoleRepo;
            _jwtSecretKey = _config["Jwt:Key"] ?? "";
        }
        [HttpGet]
        [Route("DashBoard")]
        public async Task<IActionResult> DashBoard()
        {
            IGeneralResult<List<DashboardVm>> result = new GeneralResult<List<DashboardVm>>();
            try
            {
                List<DashboardVm> dashboard=new List<DashboardVm>();

            var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
            {
                var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());

            }
            
            var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
            var applicantData=_applicantRepo.GetAllQuerable().Where(x=>x.IsDeleted==false && ((LoggedInUserRole.RoleId == (int)AppRole.Admin || LoggedInUserRole.RoleId == (int)AppRole.Admin_User) ? true:x.ClientId==Convert.ToInt32(LoggedInUserClientId))).ToList();
            var collegesData = _collegeRepo.GetAllQuerable().Where(z => z.IsDeleted == false && ((LoggedInUserRole.RoleId == (int)AppRole.Admin || LoggedInUserRole.RoleId == (int)AppRole.Admin_User) ? true : z.ClientId == Convert.ToInt32(LoggedInUserClientId))).ToList();
            var campusdata = _campusRepo.GetAllQuerable().Where(z => z.IsDeleted == false && ((LoggedInUserRole.RoleId == (int)AppRole.Admin || LoggedInUserRole.RoleId == (int)AppRole.Admin_User) ? true : z.ClientId == Convert.ToInt32(LoggedInUserClientId))).ToList();
            var questionData = _question.GetAllQuerable().Where(z => z.IsDeleted == false && ((LoggedInUserRole.RoleId == (int)AppRole.Admin || LoggedInUserRole.RoleId == (int)AppRole.Admin_User) ? true : z.ClientId == Convert.ToInt32(LoggedInUserClientId))).ToList();
            var userData = _userRepo.GetAllQuerable().Where(z => z.IsDelete == false && ((LoggedInUserRole.RoleId == (int)AppRole.Admin || LoggedInUserRole.RoleId == (int)AppRole.Admin_User) ? true : z.ClientId == Convert.ToInt32(LoggedInUserClientId))).ToList();

            if (LoggedInUserRole.RoleId == (int)AppRole.Admin || LoggedInUserRole.RoleId == (int)AppRole.Admin_User)
            {
                var clientData = _clientRepo.GetAllQuerable().Where(z => z.IsDeleted == false).ToList();
                dashboard.Add(new DashboardVm
                {
                    Title = "Total Clients",
                    TotalRecord = clientData.Count,
                    Icon=""
                });
                dashboard.Add(new DashboardVm
                {
                    Title = "Total Active Clients",
                    TotalRecord = clientData.Where(x=>x.IsActive==true).Count(),
                    Icon = ""
                });
                
            }
            dashboard.Add(new DashboardVm
            {
                Title = "Total Applicant",
                TotalRecord = applicantData.Count,
                Icon = ""
            });
            dashboard.Add(new DashboardVm
            {
                Title = "Total Active Applicant",
                TotalRecord = applicantData.Where(x => x.IsActive == true).Count(),
                Icon = ""
            });
            dashboard.Add(new DashboardVm
            {
                Title = "Total Question",
                TotalRecord = questionData.Count,
                Icon = ""
            });
            dashboard.Add(new DashboardVm
            {
                Title = "Total Active Question",
                TotalRecord = questionData.Where(x => x.IsActive == true).Count(),
                Icon = ""
            });
            dashboard.Add(new DashboardVm
            {
                Title = "Total Campus",
                TotalRecord = campusdata.Count,
                Icon = ""
            });
            dashboard.Add(new DashboardVm
            {
                Title = "Total Active Campus",
                TotalRecord = campusdata.Where(x => x.IsActive == true).Count(),
                Icon = ""
            });
            dashboard.Add(new DashboardVm
            {
                Title = "Total College",
                TotalRecord = collegesData.Count,
                Icon = ""
            });
            dashboard.Add(new DashboardVm
            {
                Title = "Total Active College",
                TotalRecord = collegesData.Where(x => x.IsActive == true).Count(),
                Icon = ""
            });

            dashboard.Add(new DashboardVm
            {
                Title = "Total Users",
                TotalRecord = userData.Count,
                Icon = ""
            });
            dashboard.Add(new DashboardVm
            {
                Title = "Total Active Users",
                TotalRecord = userData.Where(x => x.IsActive == true).Count(),
                Icon = ""
            });
                result.Data = dashboard;
                result.IsSuccess = true;
            }
            
            catch (Exception ex)
            {
                result.Message = "Server error " + ex.Message;
            }
            
            return Ok(result);
        }
        
    }
}
