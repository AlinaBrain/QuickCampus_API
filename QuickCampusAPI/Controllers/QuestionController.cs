using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {
        private readonly IQuestion _questionrepo;

        public QuestionController(IConfiguration configuration, IQuestion questionrepo)
        {
            _questionrepo = questionrepo;
        }
      

        [HttpGet]
        [Route("QuestionManage")]
        public async Task<ActionResult> QuestionManage()
        {
            var model = await _questionrepo.GetAllQuestion();
            return Ok(model);
        }

        [HttpGet]
        [Route("QuestionAdd")]
        public async Task<ActionResult> QuestionAdd()
        {
            var model = new QuestionViewModelAdmin()
            {
                QuestionTypes =  (await _questionrepo.GetAllQuestionType()).Select(x => new SelectListItem() { Value = x.QuestionTypeId.ToString(), Text = x.Questiontype }),
                Sections = (await _questionrepo.GetAllSection()).Select(x => new SelectListItem() { Value = x.SectionId.ToString(), Text = x.SectionName }), 
                Groups = (await _questionrepo.GetAllGroups()).Select(x => new SelectListItem() { Value = x.GroupId.ToString(), Text = x.GroupName }), options = new List<OptionViewModelAdmin>() { new OptionViewModelAdmin(), new OptionViewModelAdmin() } 
            };
            return Ok(model);
        }


        [HttpGet]
        [Route("QuestionEdit")]
        public async Task<ActionResult> QuestionAdd(int id)
        {
            var model = await _questionrepo.GetQuestionById(id);
            model.Groups =  (await _questionrepo.GetAllGroups()).Select(x => new SelectListItem() { Value = x.GroupId.ToString(), Text = x.GroupName });
            model.QuestionTypes = (await _questionrepo.GetAllQuestionType()).Select(x => new SelectListItem() { Value = x.QuestionTypeId.ToString(), Text = x.Questiontype });
            model.Sections = (await _questionrepo.GetAllSection()).Select(x => new SelectListItem() { Value = x.SectionId.ToString(), Text = x.SectionName });
            return Ok(model);
        }
    }
}
