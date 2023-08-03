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
        public async Task<ActionResult> QuestionManage(int clientid )
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

            var result = await _questionrepo.GetAllQuestion(cid, isSuperAdmin);
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
        [HttpGet]
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
        public async Task<ActionResult> AddOrUpdateQuestion(QuestionViewModelAdmin model, int clientid )
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
        [HttpPost]
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
