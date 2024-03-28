using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionoptionController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IQuestionOptionRepo _optionrepo;
        private string baseUrl;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly string basepath;

        public byte[] OptionImage { get;  set; }

        public QuestionoptionController(IQuestionOptionRepo optionRepo,IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _config= configuration;
            _optionrepo= optionRepo;
            baseUrl = _config.GetSection("APISitePath").Value;
            _hostingEnvironment = hostingEnvironment;

        }
        [HttpPost]
        [Route("AddQuestionOption")]
        public async Task<IActionResult> AddQuestionOption([FromForm] OptionsVm optionvm, int clientid)
        {
            IGeneralResult<OptionsVm> result = new GeneralResult<OptionsVm>();
            var _jwtSecretKey = _config["Jwt:Key"];
            int cid = 0;
            var jwtSecretKey = _config["Jwt:Key"];
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

            if (ModelState.IsValid)
            {
                OptionsVm optionvmm = new OptionsVm
                {
                   QuestionId=optionvm.QuestionId,
                   OptionId= optionvm.OptionId,
                   ImagePath = optionvm.ImagePath != null ? ProcessUploadFile(optionvm) :optionvm.ImagePath,
                   OptionText=optionvm.OptionText,
                   IsCorrect =optionvm.IsCorrect,
                   
                };
                var questiondetail = await _optionrepo.Add(optionvmm.ToQuestionOptionVmDbModel());
                result.IsSuccess = true;
                result.Message = "Question added successfully.";
                result.Data = (OptionsVm)optionvmm;
                return Ok(result);
            }
            else
            {
                result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
            }
            return Ok(result);
        }



        private string ProcessUploadFile([FromForm] OptionsVm model)
        {
            List<string> url = new List<string>();
            string uniqueFileName = null;
            if (model.Image != null)
            {
                string photoUoload = Path.Combine(_hostingEnvironment.WebRootPath, "UploadFiles");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filepath = Path.Combine(photoUoload, uniqueFileName);
                using (var filename = new FileStream(filepath, FileMode.Create))
                {
                    model.Image.CopyTo(filename);
                }
            }

            url.Add(Path.Combine(baseUrl, uniqueFileName));
            return url.FirstOrDefault();
        }
    }
}
