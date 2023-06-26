using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Interfaces;

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
        public ActionResult QuestionManage()
        {
            var model = _questionrepo.GetAllQuestion();
            return Ok(model);
        }
    }
}
