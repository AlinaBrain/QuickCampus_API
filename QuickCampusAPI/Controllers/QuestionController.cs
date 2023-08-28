using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {
        private readonly IQuestion _questionrepo;
        private IConfiguration _config;

        public QuestionController(IConfiguration configuration, IQuestion questionrepo)
        {
            _questionrepo = questionrepo;
            _config = configuration;
        }

        [Authorize(Roles = "QuestionManage")]
        [HttpGet]
        [Route("QuestionManage")]
        public async Task<ActionResult> GetAllQuestion(int clientid , int pageStart=1, int pageSize=10)
        {
            int cid = 0;
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var newPageStart = 0;
            if (pageStart > 0)
            {
                var startPage = 1;
                newPageStart = (pageStart - startPage) * pageSize;
            }
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
            }

            var result = await _questionrepo.GetAllQuestion(cid, isSuperAdmin, newPageStart, pageSize);
            return Ok(result);
        }

        [Authorize(Roles = "GetAllGroups")]
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

        [Authorize(Roles = "GetAllQuestionTypes")]
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

        [Authorize(Roles = "GetAllSectionList")]
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

        [Authorize(Roles = "QuestionActiveInactive")]
        [HttpGet]
        [Route("QuestionActiveInactive")]
        public async Task<ActionResult> QuestionActiveInactive(bool isActive, int questionId, int clientid )
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

        [Authorize(Roles = "DeleteQuestion")]
        [HttpDelete]
        [Route("deletequestion")]
        public async Task<ActionResult> DeleteQuestion(bool isdelete, int questionId, int clientid )
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


        [Authorize(Roles = "AddOrUpdateQuestion")]
        [HttpPost]
        [Route("addorupdatequestion")]
        public async Task<ActionResult> AddOrUpdateQuestion( QuestionViewModelAdmin model, int clientid )
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
            model.ClientId = cid;
            var res = model.QuestionId > 0 ? await _questionrepo.UpdateQuestion(model, isSuperAdmin) : await _questionrepo.AddQuestion(model, isSuperAdmin);
            return Ok(res);
        }

        [Authorize(Roles = "AddOrUpdateQuestion")]
        [HttpGet]
        [Route("getquestionbyid")]
        public async Task<ActionResult> GetQuestionByid(int questionId, int clientid )
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
