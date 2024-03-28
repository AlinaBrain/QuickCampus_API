using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using static QuickCampus_Core.Common.common;

namespace QuickCampusAPI.Controllers
{
    [Authorize(Roles = "Admin,Client,Client_User")]
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {
        private readonly IQuestion _questionrepo;
        private IConfiguration _config;
        private readonly IUserRepo _userRepo;
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private string _jwtSecretKey;
        private readonly IQuestionOptionRepo _questionOptionRepo;

        public QuestionController(IConfiguration configuration, IQuestion questionrepo,  IUserAppRoleRepo userAppRoleRepo, IUserRepo userRepo,IQuestionOptionRepo questionOptionRepo)
        {
            _questionrepo = questionrepo;
            _config = configuration;
            _userRepo = userRepo;
            _userAppRoleRepo = userAppRoleRepo;
            _jwtSecretKey = _config["Jwt:Key"] ?? "";
            _questionOptionRepo = questionOptionRepo;
        }

       
        [HttpGet]
        [Route("QuestionManage")]
        public async Task<ActionResult> GetAllQuestion(string? search, DataTypeFilter DataType, int pageStart = 1, int pageSize = 10) 
        {
            IGeneralResult<List<GetAllQuestionViewModel>> result = new GeneralResult<List<GetAllQuestionViewModel>>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = user.ClientId.ToString();
                }
                var newPageStart = 0;
                if (pageStart > 0)
                {
                    var startPage = 1;
                    newPageStart = (pageStart - startPage) * pageSize;
                }

                var questionTotalCount = 0;
                List<Question> questionList = new List<Question>();
                List<Question> questionData = new List<Question>();
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    questionData = _questionrepo.GetAllQuerable().Where(x => x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                else
                {
                    questionData = _questionrepo.GetAllQuerable().Where(x => x.ClentId == Convert.ToInt32(LoggedInUserClientId) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
               
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim();
                }
                questionList = questionData.Where(x => (x.Text.Contains(search ?? "", StringComparison.OrdinalIgnoreCase)  )).OrderByDescending(x => x.QuestionId).ToList();
                questionTotalCount = questionList.Count;
                questionList = questionList.Skip(newPageStart).Take(pageSize).ToList();
                //var response = questionList.Select(x => (GetAllQuestionViewModel)x).ToList();
                List<GetAllQuestionViewModel> data = new List<GetAllQuestionViewModel>();
                data.AddRange(questionList.Select(x => new GetAllQuestionViewModel
                {
                    QuestionId=x.QuestionId,
                    QuestionTypeId=x.QuestionTypeId,
                    SectionId=x.SectionId,
                    GroupId=x.GroupId,
                    Text=x.Text,
                    Marks=x.Marks
                }));
                foreach (var item in data)
                {
                    var optiondata= _questionOptionRepo.GetAllQuerable().Where(y => y.QuestionId == item.QuestionId).Select(z => new QuestionsOptionVm
                    {
                        OptionText = z.OptionText,
                        OptionId = z.OptionId,
                        Imagepath = z.Imagepath,
                        IsCorrect = z.IsCorrect
                    }).ToList();
                    item.QuestionssoptionVm = optiondata;
                }
                if (questionList.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Question get successfully";
                    result.Data = data;
                    result.TotalRecordCount = questionTotalCount;
                }
                else
                {
                    result.Message = "No applicant found!";
                }
            }
            catch (Exception ex)
            {
                result.Message = "server error! " + ex.Message;
            }
            return Ok(result);
        }

        
        [HttpGet]
        [Route("GetAllGroups")]
        public async Task<ActionResult> GetAllGroups(int clientid)
        {
            int cid = 0;
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
            }

            var result = await _questionrepo.GetAllGroups(isSuperAdmin, cid);
            return Ok(result);
        }

      
        [HttpGet]
        [Route("GetAllQuestionTypes")]
        public async Task<ActionResult> GetAllQuestionTypes(int clientid)
        {
            int cid = 0;
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
            }

            var result = await _questionrepo.GetAllQuestionTypes(isSuperAdmin, cid);
            return Ok(result);
        }

       
        [HttpGet]
        [Route("GetAllSectionList")]
        public async Task<ActionResult> GetAllSectionList(int clientid)
        {
            int cid = 0;
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
            }

            var result = await _questionrepo.GetAllSectionList(isSuperAdmin, cid);
            return Ok(result);
        }

        
        [HttpGet]
        [Route("QuestionActiveInactive")]
        public async Task<ActionResult> QuestionActiveInactive(bool isActive, int questionId, int clientid)
        {
            int cid = 0;
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
            }
            var result = await _questionrepo.ActiveInactiveQuestion(questionId, cid, isSuperAdmin, isActive);

            return Ok(result);
        }

     
        [HttpDelete]
        [Route("deletequestion")]
        public async Task<ActionResult> DeleteQuestion(bool isdelete, int questionId, int clientid)
        {
            int cid = 0;
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
            }
            var result = await _questionrepo.DeleteQuestion(questionId, cid, isSuperAdmin, isdelete);

            return Ok(result);
        }


        
        [HttpPost]
        [Route("addorupdatequestion")]
        public async Task<ActionResult> AddOrUpdateQuestion( [FromForm] QuestionTakeViewModel model, int clientid)
        {
            int cid = 0;
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
            }
            model.ClentId = cid;
            var res = _questionrepo.AddOrUpdateQuestion(model);
            return Ok(res);
        }

       
        [HttpGet]
        [Route("getquestionbyid")]
        public async Task<ActionResult> GetQuestionByid(int questionId, int clientid)
        {
            int cid = 0;
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
            }
            var res = await _questionrepo.GetQuestionById(questionId, cid, isSuperAdmin);
            return Ok(res);
        }
    }
}
