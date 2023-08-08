using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class ApplicantController : ControllerBase
    {

        private readonly IApplicantRepo _applicantRepo;
        private readonly IClientRepo clientRepo;
        private IConfiguration config;
        public ApplicantController(IApplicantRepo applicantRepo, IClientRepo clientRepo, IConfiguration config)
        {
            _applicantRepo = applicantRepo;
            this.clientRepo = clientRepo;
            this.config = config;
        }

        [Authorize(Roles = "AddApplicant")]
        [HttpPost]
        [Route("AddApplicant")]
        public async Task<IActionResult> AddApplicant(ApplicantViewModel vm, int clientid)
        {
            IGeneralResult<ApplicantViewModel> result = new GeneralResult<ApplicantViewModel>();
            var _jwtSecretKey = config["Jwt:Key"];
            int cid = 0;
            var jwtSecretKey = config["Jwt:Key"];
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

            if (_applicantRepo.Any(x => x.EmailAddress == vm.EmailAddress && x.IsActive == true && x.IsDeleted == false))
            {
                result.Message = "Email Address Already Registered!";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    ApplicantViewModel applicantViewModel = new ApplicantViewModel
                    {
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        EmailAddress = vm.EmailAddress,
                        PhoneNumber = vm.PhoneNumber,
                        HigestQualification = vm.HigestQualification,
                        HigestQualificationPercentage = vm.HigestQualificationPercentage,
                        MatricPercentage = vm.MatricPercentage,
                        IntermediatePercentage = vm.IntermediatePercentage,
                        Skills = vm.Skills,
                        StatusId = vm.StatusId ?? 0,
                        Comment = vm.Comment,
                        CollegeName = vm.CollegeName,
                        ClientId = vm.ClientId,
                    };
                   
                    var dataWithClientId = await _applicantRepo.Add(applicantViewModel.ToUpdateDbModel());
                    result.IsSuccess = true;
                    result.Message = "Applicant added successfully.";
                    result.Data = (ApplicantViewModel)dataWithClientId;
                    return Ok(result);
                }
                else
                {
                    result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                }
            }

            return Ok(result);

        }

        [Authorize(Roles = "activeAndInActiveUser")]
        [HttpGet]
        [Route("activeAndInactive")]
        public async Task<IActionResult> ActiveAndInactive(bool isActive, int id, int clientid)
        {
            IGeneralResult<ApplicantViewModel> result = new GeneralResult<ApplicantViewModel>();
            int cid = 0;
            var jwtSecretKey = config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);

                if (cid == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "Invalid User";
                    return Ok(result);
                }
            }

            var res = _applicantRepo.ActiveInActiveRole(isActive, id, cid, isSuperAdmin);
            return Ok(res);
        }
    }
}

