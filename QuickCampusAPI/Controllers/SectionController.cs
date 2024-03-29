//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using QuickCampus_Core.Common;
//using QuickCampus_Core.Interfaces;
//using QuickCampus_Core.Services;
//using QuickCampus_Core.ViewModel;
//using QuickCampus_DAL.Context;
//using System.Data;
//using System.Security.Cryptography.X509Certificates;
//using System.Text.RegularExpressions;

//namespace QuickCampusAPI.Controllers
//{
//    [Authorize]
//    [Route("api/[controller]")]
//    [ApiController]
//    public class SectionController : ControllerBase
//    {
//        private readonly ISectionRepo _sectionRepo;
//        private readonly IUserAppRoleRepo _userAppRoleRepo;
//        private readonly IUserRepo _userRepo;
//        private readonly IConfiguration _config;
//        private string _jwtSecretKey;

//        public SectionController(ISectionRepo sectionRepo,IUserAppRoleRepo userAppRoleRepo,IUserRepo userRepo, IConfiguration configuration)
//        {
//           _sectionRepo= sectionRepo;
//            _userAppRoleRepo = userAppRoleRepo;
//            _userRepo = userRepo;
//            _config = configuration;
//            _jwtSecretKey = _config["Jwt:Key"] ?? "";
//        }
        
//        [HttpGet]
//        [Route("GetAllSection")]
//        public async Task<IActionResult> GetAllSection()
//        {
//            IGeneralResult<List<SectionVm>> result = new GeneralResult<List<SectionVm>>();
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
//                List<Section> SectionList = new List<Section>();
//                SectionList = (await _sectionRepo.GetAll()).ToList();
//                var response = SectionList.Select(x => (SectionVm)x).ToList();
//                if (response.Count > 0)
//                {
//                    result.IsSuccess = true;
//                    result.Message = "Section fetched successfully";
//                    result.Data = response;
//                    result.TotalRecordCount = response.Count;
//                }
//                else
//                {
//                    result.Message = "Section list not found!";
//                }
//            }
//            catch (Exception ex)
//            {
//                result.Message = ex.Message;
//            }
//            return Ok(result);
//        }

//        //[Authorize(Roles = "AddSection")]
//        //[HttpPost]
//        //[Route("AddSection")]
//        //public async Task<IActionResult> AddSection(SectionVm vm, int clientid)
//        //{
//        //    IGeneralResult<SectionVm> result = new GeneralResult<SectionVm>();
//        //    var _jwtSecretKey = _config["Jwt:Key"];
//        //    int cid = 0;
//        //    var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
//        //    var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
//        //    if (isSuperAdmin)
//        //    {
//        //        cid = clientid;
//        //    }
//        //    else
//        //    {
//        //        cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
//        //    }
//        //    var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
//        //    if (vm != null)
//        //    {

//        //        if (ModelState.IsValid)
//        //        {
//        //            string pattern = @"^[a-zA-Z][a-zA-Z\s]*$";
//        //            string input = vm.Section1;
//        //            Match m = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
//        //            if (!m.Success)
//        //            {
//        //                result.Message = "Only alphabetic characters are allowed in the name.";
//        //                return Ok(result);
//        //            }


//        //            {
//        //                SectionVm sectionVm = new SectionVm()
//        //                {
//        //                    Section1 = vm.Section1,
//        //                    SectionId = cid,
//        //                    SortOrder = vm.SortOrder,
//        //                    ClentId = cid,
                               
//        //                    };
//        //                    try
//        //                    {
//        //                        var sectiondata = await _sectionRepo.Add(sectionVm.ToSectionDbModel());
//        //                        result.Data = (SectionVm)sectiondata;
//        //                        result.Message = "MstCity added successfully";
//        //                        result.IsSuccess = true;
//        //                    }

//        //                    catch (Exception ex)
//        //                    {
//        //                        result.Message = ex.Message;
//        //                    }
//        //                    return Ok(result);
//        //                }
//        //            }
//        //        }
//        //        else
//        //        {
//        //            result.Message = "something Went Wrong";
//        //        }
//        //    return Ok(result);
//        //    }
           
//        }
//    }

