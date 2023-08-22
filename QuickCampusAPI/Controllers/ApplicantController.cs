using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Common.Enum;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class ApplicantController : ControllerBase
    {

        private readonly IApplicantRepo _applicantRepo;
        private readonly IConfiguration _config;

        public ApplicantController(IConfiguration configuration, IApplicantRepo applicantRepo)
        {
            _applicantRepo = applicantRepo;
            _config = configuration;
        }

        [Authorize(Roles = "GetAllApplicant")]
        [HttpGet]
        [Route("GetAllApplicant")]
        public async Task<ActionResult> GetAllApplicant(int clientid, int pageStart=1,int pageSize=10)
        {
            IGeneralResult<List<ApplicantViewModel>> result = new GeneralResult<List<ApplicantViewModel>>();
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
            List<Applicant> apllicantlist = new List<Applicant>();
            var applicantTotalCount = 0;
            try
            {
                if (isSuperAdmin)
                {
                    applicantTotalCount = (await _applicantRepo.GetAll()).Where(x => x.IsDeleted != true && (cid == 0 ? true : x.ClientId == cid)).Count();
                    apllicantlist = (await _applicantRepo.GetAll()).Where(x => x.IsDeleted != true && (cid == 0 ? true : x.ClientId == cid)).Skip(newPageStart).Take(pageSize).ToList();
                }
                else
                {
                    applicantTotalCount = (await _applicantRepo.GetAll()).Where(x => x.IsDeleted != true && x.ClientId == cid).Count();
                    apllicantlist = (await _applicantRepo.GetAll()).Where(x => x.IsDeleted != true && x.ClientId == cid).OrderByDescending(x=>x.ApplicantId).Skip(newPageStart).Take(pageSize).ToList();
                }
                var response = apllicantlist.Select(x => (ApplicantViewModel)x).ToList();
                if (apllicantlist.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Applicant get successfully";
                    result.Data = response;
                    result.TotalRecordCount = applicantTotalCount;
                }
                else
                {
                    result.IsSuccess = true;
                    result.Message = "Applicant list not found!";
                    result.Data = null;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }

        [Authorize(Roles = "AddApplicant")]
        [HttpPost]
        [Route("AddApplicant")]
        public async Task<IActionResult> AddApplicant(ApplicantViewModel vm, int clientid)
        {
            IGeneralResult<ApplicantViewModel> result = new GeneralResult<ApplicantViewModel>();
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

            if (_applicantRepo.Any(x => x.EmailAddress == vm.EmailAddress && x.IsActive == true && x.IsDeleted == false && x.ApplicantId==vm.ApplicantID))
            {
                result.Message = "Email Address Already Registered!";
                return Ok(result);
            }
            bool isExits = _applicantRepo.Any(x => x.FirstName == vm.FirstName && x.IsDeleted == false && x.ApplicantId == vm.ApplicantID);
            if (isExits)
            {
                result.Message = " FirstName is already exists";
                return Ok(result);
            }

            bool isphonenumberexist = _applicantRepo.Any(x => x.PhoneNumber == vm.PhoneNumber && x.IsDeleted == false && x.ApplicantId == vm.ApplicantID);
            if (isphonenumberexist)
            {
                result.Message = "Phone Number  is Already Exist";
                return Ok(result);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    ApplicantViewModel applicantViewModel = new ApplicantViewModel
                    {
                        FirstName = vm.FirstName.Trim(),
                        LastName = vm.LastName.Trim(),
                        EmailAddress = vm.EmailAddress.Trim(),
                        PhoneNumber = vm.PhoneNumber.Trim(),
                        HigestQualification = vm.HigestQualification,
                        HigestQualificationPercentage = vm.HigestQualificationPercentage,
                        MatricPercentage = vm.MatricPercentage,
                        IntermediatePercentage = vm.IntermediatePercentage,
                        Skills = vm.Skills,
                        StatusId = (int)(StatusEnum)vm.StatusId,
                        Comment = vm.Comment.Trim(),
                        CollegeName = vm.CollegeName.Trim(),
                        ClientId = cid,
                        AssignedToCompany= (int)(CompanyEnum)vm.AssignedToCompany,
                        CollegeId =vm.CollegeId

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

        [Authorize(Roles = "EditApplicant")]
        [HttpPost]
        [Route("EditApplicant")]
        public async Task<IActionResult> EditApplicant(ApplicantViewModel vm, int clientid)
        {
            IGeneralResult<ApplicantViewModel> result = new GeneralResult<ApplicantViewModel>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);

            int cid = 0;
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
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
                    result.Message = "Invalid Client";
                    return Ok(result);
                }
            }
            if (vm != null)
            {
                Applicant applicant = new Applicant();

                if (isSuperAdmin)
                {
                    applicant = (await _applicantRepo.GetAll()).Where(w => w.IsDeleted == false && w.IsActive == true && cid == 0 ? true : w.ClientId == cid && w.ApplicantId==vm.ApplicantID).FirstOrDefault();
                }
                else
                {
                    applicant = (await _applicantRepo.GetAll()).Where(w => w.IsDeleted == false && w.IsActive == true && w.ClientId == cid && w.ApplicantId == vm.ApplicantID).FirstOrDefault();
                }
                if (applicant == null)
                {
                    result.IsSuccess = false;
                    result.Message = " Applicant does Not Exist";
                    return Ok(result);
                }
                bool isDeleted = (bool)applicant.IsDeleted ? true : false;
                if (isDeleted)
                {
                    result.IsSuccess = false;
                    result.Message = " Applicant does Not Exist";
                    return Ok(result);
                }
                bool isExits = _applicantRepo.Any(x => x.FirstName == vm.FirstName && x.IsDeleted == false && x.ApplicantId==vm.ApplicantID);
                if (isExits)
                {
                    result.Message = " FirstName is already exists";
                    return Ok(result);
                }

                //bool isphonenumberexist = _applicantRepo.Any(x => x.PhoneNumber == vm.PhoneNumber && x.IsDeleted == false);
                //if (isphonenumberexist)
                //{
                //    result.Message = "Phone Number  is Already Exist";
                //    return Ok(result);
                //}
                bool isemailAddress = _applicantRepo.Any(x => x.EmailAddress == vm.EmailAddress && x.IsDeleted == false);
                if (isemailAddress)
                {
                    result.Message = "Email Address is Already Exist";
                    return Ok(result);
                }
                else
                {
                    if (ModelState.IsValid && vm.ApplicantID > 0 && applicant.IsDeleted == false)
                    {
                        applicant.ApplicantId = vm.ApplicantID;
                        applicant.CollegeName = vm.CollegeName.Trim();
                        applicant.FirstName = vm.FirstName.Trim();
                        applicant.LastName = vm.LastName.Trim();
                        applicant.EmailAddress = vm.EmailAddress.Trim();
                        applicant.HigestQualification = vm.HigestQualification.Trim();
                        applicant.IntermediatePercentage = vm.IntermediatePercentage;
                        applicant.ClientId = cid;
                        applicant.HigestQualificationPercentage = vm.HigestQualificationPercentage;
                        applicant.Skills = vm.Skills.Trim();
                        applicant.MatricPercentage = vm.MatricPercentage;
                        applicant.PhoneNumber = vm.PhoneNumber.Trim();
                        applicant.StatusId = (int)(StatusEnum)vm.StatusId;
                        applicant.AssignedToCompany =(int)(CompanyEnum)vm.AssignedToCompany;
                        applicant.CollegeId = vm.CollegeId;
                        applicant.Comment = vm.Comment;
                        applicant.ModifiedDate = DateTime.Now;
                        try
                        {
                            result.Data = (ApplicantViewModel)await _applicantRepo.Update(applicant);
                            result.Message = "Applicant updated successfully";
                            result.IsSuccess = true;
                        }
                        catch (Exception ex)
                        {
                            result.Message = ex.Message;
                        }
                        return Ok(result);
                    }
                    else
                    {
                        result.Message = "something Went Wrong";
                    }
                }
            }
            return Ok(result);
        }

        [Authorize(Roles = "GetApplicantById")]
        [HttpGet]
        [Route("GetApplicantById")]
        public async Task<ActionResult> GetApplicantById(int applicantId, int clientid)
        {
            IGeneralResult<ApplicantViewModel> result = new GeneralResult<ApplicantViewModel>();
            var _jwtSecretKey = _config["Jwt:Key"];
            int cid = 0;
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

            var res = await _applicantRepo.GetById(applicantId);
            if (res.IsDeleted == false && res.IsActive == true)
            {
                result.Data = (ApplicantViewModel)res;
                result.IsSuccess = true;
                result.Message = "Applicant details getting succesfully";
            }
            else
            {
                result.Message = "Applicant does Not exist";
            }
            return Ok(result);
        }

        [Authorize(Roles = "DeleteApplicant")]
        [HttpDelete]
        [Route("DeleteApplicant")]
        public async Task<IActionResult> DeleteApplicant(int applicantId,int clientid, bool isDeleted)
        {
            IGeneralResult<ApplicantViewModel> result = new GeneralResult<ApplicantViewModel>();
            int cid = 0;
            var jwtSecretKey = _config["Jwt:Key"];
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
                    result.Message = "Invalid Applicant";
                    return Ok(result);
                }
            }
            var res = await _applicantRepo.DeleteApplicant(isDeleted, applicantId, cid, isSuperAdmin);
            return Ok(res);
        }

        [Authorize(Roles = "activeAndInActiveUser")]
        [HttpGet]
        [Route("activeAndInactive")]
        public async Task<IActionResult> ActiveAndInactive(bool isActive, int id, int clientid)
        {
            IGeneralResult<ApplicantViewModel> result = new GeneralResult<ApplicantViewModel>();
            int cid = 0;
            var jwtSecretKey = _config["Jwt:Key"];
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

