using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using System.Web.Http.Results;
using System.Xml;
using static QuickCampus_Core.Common.common;

namespace QuickCampusAPI.Controllers
{
    [Authorize(Roles = "Admin,Client,Client_User,Admin_User")]
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
        private readonly IMstMeneItemRepo _mstMeneItemRepo;

        public DashboardController(IApplicantRepo applicantRepo, ICollegeRepo collegeRepo, IQuestion question, ICampusRepo campusRepo, IClientRepo clientRepo, IUserRepo userRepo, IUserAppRoleRepo userAppRoleRepo, IConfiguration configuration, IMstMeneItemRepo mstMeneItemRepo)
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
            _mstMeneItemRepo = mstMeneItemRepo;
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

                    List<DashVm> ClientData = new List<DashVm>();
                    ClientData.Add(new DashVm
                    {
                        Title = "Total Clients",
                        TotalRecord = clientData.Count
                    });
                    ClientData.Add(new DashVm
                    {
                        Title = " Active Clients",
                        TotalRecord = clientData.Where(x => x.IsActive == true).Count()
                    });
                    var client = _mstMeneItemRepo.GetAllQuerable().Where(y => y.ItemName == "Client").FirstOrDefault();
                    dashboard.Add(new DashboardVm
                    {
                        DashData = ClientData,
                        Icon = client.ItemIcon,
                        Url = client.ItemUrl


                    });
                }
                    List<DashVm> ApplicantData = new List<DashVm>();
                    ApplicantData.Add(new DashVm
                    {
                        Title = "Total Applicant",
                        TotalRecord = applicantData.Count
                    });
                  
                    ApplicantData.Add(new DashVm
                    {
                        Title = " Active Applicant",
                        TotalRecord = applicantData.Where(x => x.IsActive == true).Count()
                    });
                    var applicant = _mstMeneItemRepo.GetAllQuerable().Where(y => y.ItemName == "Applicant").FirstOrDefault();
                    dashboard.Add(new DashboardVm
                    {
                        DashData = ApplicantData,
                        Icon = applicant.ItemIcon,
                        Url=applicant.ItemUrl
                    });
                    List<DashVm> QuestionData = new List<DashVm>();
                    QuestionData.Add(new DashVm
                    {
                        Title = "Total Question",
                        TotalRecord = questionData.Count
                    });
                    QuestionData.Add(new DashVm
                    {
                        Title = " Active Question",
                        TotalRecord = questionData.Where(x => x.IsActive == true).Count()
                    });
                    var question = _mstMeneItemRepo.GetAllQuerable().Where(y => y.ItemName == "Question").FirstOrDefault();
                    dashboard.Add(new DashboardVm
                    {
                        DashData = QuestionData,
                        Icon = question.ItemIcon,
                        Url=question.ItemUrl
                    });
                    List<DashVm> UserData = new List<DashVm>();
                    UserData.Add(new DashVm
                    {
                        Title = "Total Users",
                        TotalRecord = userData.Count
                    });
                    UserData.Add(new DashVm
                    {
                        Title = " Active Users",
                        TotalRecord = userData.Where(x => x.IsActive == true).Count()
                    });
                    var userres = _mstMeneItemRepo.GetAllQuerable().Where(y => y.ItemName == "User").FirstOrDefault();
                    dashboard.Add(new DashboardVm
                    {
                        DashData = UserData,
                        Icon = userres.ItemIcon,
                        Url= userres.ItemUrl
                    });

                    List<DashVm> CampusData = new List<DashVm>();
                    CampusData.Add(new DashVm
                    {
                        Title = "Total Campus",
                        TotalRecord = campusdata.Count
                    });
                    CampusData.Add(new DashVm
                    {
                        Title = " Active Campus",
                        TotalRecord = campusdata.Where(x => x.IsActive == true).Count()
                    });
                    var campus = _mstMeneItemRepo.GetAllQuerable().Where(y => y.ItemName == "CampusWalkIn").FirstOrDefault();
                    dashboard.Add(new DashboardVm
                    {
                        DashData = CampusData,
                        Icon = campus.ItemIcon,
                        Url=campus.ItemUrl
                    });


                    List<DashVm> CollegeData = new List<DashVm>();
                    CollegeData.Add(new DashVm
                    {
                        Title = "Total College",
                        TotalRecord = collegesData.Count
                    });
                    CollegeData.Add(new DashVm
                    {
                        Title = " Active College",
                        TotalRecord = collegesData.Where(x => x.IsActive == true).Count()
                    });
                    var college = _mstMeneItemRepo.GetAllQuerable().Where(y => y.ItemName == "Colleges").FirstOrDefault();
                    dashboard.Add(new DashboardVm
                    {
                        DashData = CollegeData,
                        Icon = college.ItemIcon,
                        Url=college.ItemUrl
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
