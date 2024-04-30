//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using QuickCampus_Core.Common;
//using QuickCampus_Core.Interfaces;
//using QuickCampus_Core.Services;
//using QuickCampus_Core.ViewModel;
//using static QuickCampus_Core.Common.common;

//namespace QuickCampusAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class DashboardController : ControllerBase
//    {
//        private readonly IApplicantRepo _applicantRepo;
//        private readonly ICollegeRepo _collegeRepo;
//        private readonly IQuestion _question;
//        private readonly ICampusRepo _campusRepo;
//        private readonly IClientRepo _clientRepo;
//        private readonly IUserRepo _userRepo;

//        public DashboardController(IApplicantRepo applicantRepo,ICollegeRepo collegeRepo, IQuestion question,ICampusRepo campusRepo,IClientRepo clientRepo,IUserRepo userRepo)
//        {
//            _applicantRepo = applicantRepo;
//            _collegeRepo = collegeRepo;
//            _question = question;
//            _campusRepo = campusRepo;
//            _clientRepo = clientRepo;
//            _userRepo = userRepo;
//        }
//        public async Task<IActionResult> DashBoard(DashboardVm vm)
//        {
//            IGeneralResult<List<DashboardVm>>result= new GeneralResult<List<DashboardVm>>();
//            var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
//            var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
//            if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
//            {
//                var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
//                LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
//            }
//            var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
//            if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin && (vm.ClientId == null || vm.ClientId.ToString() == "" || vm.ClientId == 0))
//            {
//                result.Message = "Please select a valid Client";
//                return Ok(result);
//            }


//        }
        
//    }
//}
