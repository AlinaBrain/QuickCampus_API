//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using QuickCampus_Core.Common;
//using QuickCampus_Core.Interfaces;
//using QuickCampus_Core.ViewModel;

//namespace QuickCampusAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class QuestionsController : ControllerBase
//    {
//        private readonly IQuestion _questionvm;
//        private readonly IConfiguration _config;

//        public QuestionsController(IQuestion questionvm, IConfiguration configuration)
//        {
//            _questionvm = questionvm;
//            _config = configuration;
//        }
//        [HttpPost]
//        [Route("AddQuestions")]
//        public async Task<IActionResult> AddQuestions(QuestionVmm questionVmm, int clientid)
//        {
//            IGeneralResult<QuestionVmm> result = new GeneralResult<QuestionVmm>();
//            var _jwtSecretKey = _config["Jwt:Key"];
//            int cid = 0;
//            var jwtSecretKey = _config["Jwt:Key"];
//            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
//            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
//            if (isSuperAdmin)
//            {
//                cid = clientid;
//            }
//            else
//            {
//                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
//            }

//            if (ModelState.IsValid)
//            {
//                QuestionVmm questionvm = new QuestionVmm
//                {
//                    QuestionId = questionVmm.QuestionId,
//                    QuestionTypeId = questionVmm.QuestionTypeId,
//                    GroupId = questionVmm.GroupId,
//                    Text = questionVmm.Text,
//                    SectionId = questionVmm.SectionId,
//                    IsActive = questionVmm.IsActive,
//                    Marks = questionVmm.Marks,
//                    IsDeleted = questionVmm.IsDeleted,
//                    ClientId = questionVmm.ClientId,
//                };
//                var questiondetail = await _questionvm.Add(questionvm.ToQuestionDbModel());
//                result.IsSuccess = true;
//                result.Message = "Question added successfully.";
//                result.Data = (QuestionVmm)questiondetail;
//                return Ok(result);
//            }
//            else
//            {
//                result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
//            }
//            return Ok(result);
//        }
//    }
//}

