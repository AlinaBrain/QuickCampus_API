using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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


        [HttpGet]
        [Route("QuestionManage")]
        public async Task<ActionResult> QuestionManage(int clientid)
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
                cid = string.IsNullOrEmpty(clientId)?0:Convert.ToInt32(clientId);
            }

            var result = await _questionrepo.GetAllQuestion(cid,isSuperAdmin);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetQuestion")]
        public async Task<ActionResult> GetQuestion()
        {
            var model = new QuestionViewModelAdmin()
            {
                QuestionTypes = (await _questionrepo.GetAllQuestionType()).Select(x => new SelectListItem() { Value = x.QuestionTypeId.ToString(), Text = x.Questiontype }),
                Sections = (await _questionrepo.GetAllSection()).Select(x => new SelectListItem() { Value = x.SectionId.ToString(), Text = x.SectionName }),
                Groups = (await _questionrepo.GetAllGroups()).Select(x => new SelectListItem() { Value = x.GroupId.ToString(), Text = x.GroupName }),
                options = new List<OptionViewModelAdmin>() { new OptionViewModelAdmin(), new OptionViewModelAdmin() }
            };
            return Ok(model);
        }


        [HttpGet]
        [Route("QuestionEdit")]
        public async Task<ActionResult> QuestionAdd(int id)
        {
            var model = await _questionrepo.GetQuestionById(id);
            model.Groups = (await _questionrepo.GetAllGroups()).Select(x => new SelectListItem() { Value = x.GroupId.ToString(), Text = x.GroupName });
            model.QuestionTypes = (await _questionrepo.GetAllQuestionType()).Select(x => new SelectListItem() { Value = x.QuestionTypeId.ToString(), Text = x.Questiontype });
            model.Sections = (await _questionrepo.GetAllSection()).Select(x => new SelectListItem() { Value = x.SectionId.ToString(), Text = x.SectionName });
            return Ok(model);
        }


        [HttpGet]
        [Route("QuestionActiveInactive")]
        public async Task<ActionResult> QuestionActiveInactive(int id)
        {
            var result = await _questionrepo.ActiveInactiveQuestion(id);

            return Ok(result);
        }
    }
}
