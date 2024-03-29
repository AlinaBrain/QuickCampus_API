//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using QuickCampus_Core.Common;
//using QuickCampus_Core.Interfaces;
//using QuickCampus_Core.Services;
//using QuickCampus_Core.ViewModel;
//using QuickCampus_DAL.Context;
//using static QuickCampus_Core.Common.common;

//namespace QuickCampusAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class GroupController : ControllerBase
//    {
//        private readonly IGroupRepo _grouprepo;
//        private readonly IUserRepo _userRepo;
//        private readonly IUserAppRoleRepo _userAppRoleRepo;
//        private readonly IConfiguration _config;
//        private string _jwtSecretKey;

//        public GroupController( IGroupRepo groupRepo, IUserRepo userRepo,IUserAppRoleRepo userAppRoleRepo,IConfiguration configuration)
//        {
//            _grouprepo=groupRepo;
//            _userRepo = userRepo;
//            _userAppRoleRepo = userAppRoleRepo;
//            _config = configuration;
//            _jwtSecretKey = _config["Jwt:Key"] ?? "";
//        }

//        [HttpGet]
//        [Route("GetAllGroups")]
//        public async Task<IActionResult> GetAllGroup()
//        {
//            IGeneralResult<List<GroupVm>> result = new GeneralResult<List<GroupVm>>();
//            try
//            {
//                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
//                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
//                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
//                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
//                {
//                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
//                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
//                }
//                List<Groupdl> groupList = new List<Groupdl>();
//                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
//                {
//                    groupList = _grouprepo.GetAllQuerable().ToList();
//                }
//                else
//                {
//                    groupList = _grouprepo.GetAllQuerable().Where(x => x.ClientId == Convert.ToInt32(LoggedInUserClientId)).ToList();
//                }
                
//                var response = groupList.Select(x => (GroupVm)x).ToList();
//                if (groupList.Count > 0)
//                {
//                    result.IsSuccess = true;
//                    result.Message = "Groups fetched successfully";
//                    result.Data = response;
//                    result.TotalRecordCount = groupList.Count;
//                }
//                else
//                {
//                    result.Message = "Groups not found!";
//                }
//            }
//            catch (Exception ex)
//            {
//                result.Message = ex.Message;
//            }
//            return Ok(result);
//        }

//    }
//}
