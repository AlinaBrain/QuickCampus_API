using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Text.RegularExpressions;
using static QuickCampus_Core.Common.common;



namespace QuickCampusAPI.Controllers
{
    [Authorize(Roles = "Admin,Client,Client_User")]
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicantController : ControllerBase
    {

        private readonly IApplicantRepo _applicantRepo;
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private readonly IConfiguration _config;
        private readonly IMstQualificationRepo _qualificationRepo;
        private readonly ICollegeRepo _collegeRepo;
        private readonly IUserRepo _userRepo;
        private string _jwtSecretKey;

        public ApplicantController(IConfiguration configuration, IMstQualificationRepo qualificationRepo, ICollegeRepo collegeRepo, IApplicantRepo applicantRepo
            ,IUserAppRoleRepo userAppRoleRepo,IUserRepo userRepo)
        {
            _applicantRepo = applicantRepo;
            _userAppRoleRepo = userAppRoleRepo;
            _config = configuration;
            _qualificationRepo = qualificationRepo;
            _collegeRepo = collegeRepo;
            _userRepo = userRepo;
            _jwtSecretKey = _config["Jwt:Key"] ?? "";
        }

        [HttpGet]
        [Route("GetAllApplicant")]
        public async Task<ActionResult> GetAllApplicant(string? search, int? ClientId, DataTypeFilter DataType , int pageStart = 1, int pageSize = 10)
        {
            IGeneralResult<List<ApplicantViewModel>> result = new GeneralResult<List<ApplicantViewModel>>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user =await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0": user.ClientId.ToString());
                }
                var newPageStart = 0;
                if (pageStart > 0)
                {
                    var startPage = 1;
                    newPageStart = (pageStart - startPage) * pageSize;
                }

                var applicantTotalCount = 0;
                List<Applicant> applicantList = new List<Applicant>();
                List<Applicant> applicantData = new List<Applicant>();
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    applicantData = _applicantRepo.GetAllQuerable().Where(x => (ClientId != null && ClientId > 0 ? x.ClientId == ClientId : true) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                else
                {
                    applicantData = _applicantRepo.GetAllQuerable().Where(x => x.ClientId == Convert.ToInt32(LoggedInUserClientId) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                var collegeList = _collegeRepo.GetAllQuerable().Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                var qualificationList = _qualificationRepo.GetAllQuerable().Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                foreach (var item in applicantData)
                {
                    if (collegeList.Any(x => x.CollegeId == item.CollegeId))
                    {
                        item.CollegeName = collegeList.Where(x => x.CollegeId == item.CollegeId).First()?.CollegeName;
                    }
                }
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim();

                }
                applicantList = applicantData.Where(x => ((x.FirstName + " " + x.LastName).Contains(search ?? "", StringComparison.OrdinalIgnoreCase) || x.FirstName.Contains(search ?? "", StringComparison.OrdinalIgnoreCase) || x.LastName.Trim().Contains(search ?? "", StringComparison.OrdinalIgnoreCase) || x.EmailAddress.Contains(search ?? "", StringComparison.OrdinalIgnoreCase) || x.PhoneNumber.Contains(search ?? "") || x.CollegeName.Contains(search ?? "", StringComparison.OrdinalIgnoreCase))).OrderByDescending(x => x.ApplicantId).ToList();
                applicantTotalCount = applicantList.Count;
                applicantList = applicantList.Skip(newPageStart).Take(pageSize).ToList();
                var response = applicantList.Select(x => (ApplicantViewModel)x).ToList();
                foreach (var item in response)
                {
                    if (qualificationList.Any(x => x.QualId == item.HighestQualification))
                    {
                        item.HighestQualificationName = qualificationList.Where(x => x.QualId == item.HighestQualification).First()?.QualName;
                    }
                }
                if (applicantList.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Applicant get successfully";
                    result.Data = response;
                    result.TotalRecordCount = applicantTotalCount;
                }
                else
                {
                    result.Message = "No applicant found!";
                }
            }
            catch (Exception ex)
            {
                result.Message = "server error! "+ ex.Message;
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("AddApplicant")]
        public async Task<IActionResult> AddApplicant(ApplicantViewModel vm)
        {
            IGeneralResult<ApplicantViewModel> result = new GeneralResult<ApplicantViewModel>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0": user.ClientId.ToString());
                }
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if(LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin && (vm.ClientId.ToString() == "" || vm.ClientId == 0))
                {
                    result.Message = "Please select a valid Client";
                    return Ok(result);
                }

                if (_applicantRepo.Any(x => x.EmailAddress == vm.EmailAddress && x.IsActive == true && x.IsDeleted == false))
                {
                    result.Message = "Email Address Already Registered!";
                    return Ok(result);
                }
                bool isPhoneNumberExist = _applicantRepo.Any(x => x.PhoneNumber == vm.PhoneNumber && x.IsDeleted == false);
                if (isPhoneNumberExist)
                {
                    result.Message = "Phone Number is Already Exist";
                    return Ok(result);
                }
                bool isCollegeExist = _collegeRepo.Any(x => x.CollegeId == vm.CollegeId && x.IsDeleted == false && x.IsActive == true);
                if (!isCollegeExist)
                {
                    result.Message = "College does not Exist";
                    return Ok(result);
                }
                bool isQualificationExist = _qualificationRepo.Any(x => x.QualId == vm.HighestQualification && x.IsDeleted == false && x.IsActive == true);
                if (!isCollegeExist)
                {
                    result.Message = "Qualification does not Exist";
                    return Ok(result);
                }


                if (ModelState.IsValid)
                {
                    string pattern = @"^[a-zA-Z][a-zA-Z\s]*$";
                    Match m1 = Regex.Match(vm.FirstName ?? "", pattern, RegexOptions.IgnoreCase);
                    Match m2 = Regex.Match(vm.LastName ?? "", pattern, RegexOptions.IgnoreCase);
                    if (!m1.Success && !m2.Success)
                    {
                        result.Message = "Only alphabetic characters are allowed in the name.";
                        return Ok(result);
                    }
                    vm.FirstName = vm.FirstName?.Trim();
                    vm.LastName = vm.LastName?.Trim();
                    vm.EmailAddress = vm.EmailAddress?.Trim();
                    vm.PhoneNumber = vm.PhoneNumber?.Trim();
                    vm.Comment = vm.Comment?.Trim();
                    vm.HighestQualification = vm.HighestQualification;
                    vm.ClientId = (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin) ? vm.ClientId : Convert.ToInt32(LoggedInUserClientId);
                    var SaveApplicant = await _applicantRepo.Add(vm.ToApplicantDbModel());
                    if (SaveApplicant.ApplicantId > 0)
                    {
                        result.IsSuccess = true;
                        result.Message = "Applicant added successfully.";
                        result.Data = (ApplicantViewModel)SaveApplicant;
                    }
                    else
                    {
                        result.Message = "Applicant not saved. Please try again.";
                    }

                }
                else
                {
                    result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error " + ex.Message;
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("EditApplicant")]
        public async Task<IActionResult> EditApplicant(ApplicantViewModel vm)
        {
            IGeneralResult<ApplicantViewModel> result = new GeneralResult<ApplicantViewModel>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();

                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0": user.ClientId.ToString());
                }
                if (_applicantRepo.Any(x => x.EmailAddress == vm.EmailAddress &&x.ApplicantId!=vm.ApplicantID && x.IsActive == true && x.IsDeleted == false))
                {
                    result.Message = "Email Address Already Registered!";
                    return Ok(result);
                }
                bool isPhoneNumberExist = _applicantRepo.Any(x => x.PhoneNumber == vm.PhoneNumber && x.ApplicantId != vm.ApplicantID && x.IsDeleted == false);
                if (isPhoneNumberExist)
                {
                    result.Message = "Phone Number is Already Exist";
                    return Ok(result);
                }
                bool isCollegeExist = _collegeRepo.Any(x => x.CollegeId == vm.CollegeId && x.IsDeleted == false && x.IsActive == true);
                if (!isCollegeExist)
                {
                    result.Message = "College does not Exist";
                    return Ok(result);
                }
                var isQualificationExists = _qualificationRepo.Any(x => x.QualId == vm.HighestQualification && x.IsActive == true && x.IsDeleted == false);
                if (!isQualificationExists)
                {
                    result.Message = "Qualification Does Not Exist";
                    return Ok(result);
                }

                if (ModelState.IsValid)
                {
                    string pattern = @"^[a-zA-Z][a-zA-Z\s]*$";
                    Match m1 = Regex.Match(vm.FirstName ?? "", pattern, RegexOptions.IgnoreCase);
                    Match m2 = Regex.Match(vm.LastName ?? "", pattern, RegexOptions.IgnoreCase);
                    if (!m1.Success && !m2.Success)
                    {
                        result.Message = "Only alphabetic characters are allowed in the name.";
                        return Ok(result);
                    }
                    
                        if (vm.StatusId > 0 && vm.AssignedToCompany > 0 && vm.ApplicantID > 0)
                    {
                        Applicant applicant = new Applicant();

                        if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                        {
                            applicant = _applicantRepo.GetAllQuerable().Where(x => x.ApplicantId == vm.ApplicantID && x.IsDeleted == false).FirstOrDefault();
                        }
                        else
                        {
                            applicant = _applicantRepo.GetAllQuerable().Where(x => x.ApplicantId == vm.ApplicantID && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                        }
                        if (applicant == null)
                        {
                            result.Message = " Applicant does Not Exist";
                            return Ok(result);
                        }
                        applicant.FirstName = vm.FirstName?.Trim();
                        applicant.LastName = vm.LastName?.Trim();
                        applicant.EmailAddress = vm.EmailAddress?.Trim();
                        applicant.IntermediatePercentage = vm.IntermediatePercentage;
                        applicant.HighestQualification = vm.HighestQualification;
                        applicant.HigestQualificationPercentage = vm.HighestQualificationPercentage;
                        applicant.Skills = vm.Skills?.Trim();
                        applicant.MatricPercentage = vm.MatricPercentage;
                        applicant.PhoneNumber = vm.PhoneNumber?.Trim();
                        applicant.StatusId = vm.StatusId;
                        applicant.AssignedToCompany = vm.AssignedToCompany;
                        applicant.CollegeId = vm.CollegeId;
                        applicant.Comment = vm.Comment;
                        applicant.ModifiedDate = DateTime.Now;
                        await _applicantRepo.Update(applicant);
                        result.Message = "Applicant updated successfully";
                        result.IsSuccess = true;
                        return Ok(result);
                    }
                    else
                    {
                        result.Message = "Applicant, Status or Assigned to Company can't be null or Zero.";
                    }
                }
                else
                {
                    result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetApplicantById")]
        public async Task<ActionResult> GetApplicantById(int applicantId)
        {
            IGeneralResult<ApplicantViewModel> result = new GeneralResult<ApplicantViewModel>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0": user.ClientId.ToString());
                }
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();

                if (applicantId > 0)
                {
                    Applicant applicant = new Applicant();
                    var collegeList = _collegeRepo.GetAllQuerable().Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                    


                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        applicant = _applicantRepo.GetAllQuerable().Where(x => x.ApplicantId == applicantId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        applicant = _applicantRepo.GetAllQuerable().Where(x => x.ApplicantId == applicantId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (applicant == null)
                    {
                        result.Message = " Applicant does Not Exist";
                    }
                    else
                    {
                        result.IsSuccess = true;
                        result.Message = "Applicant fetched successfully.";
                        result.Data = (ApplicantViewModel)applicant;
                        var qualificationlist = _qualificationRepo.GetAllQuerable().Where(x => x.IsActive == true && x.IsDeleted == false && x.QualId == applicant.HighestQualification).FirstOrDefault();
                        var collegelist=_collegeRepo.GetAllQuerable().Where(x=>x.IsActive==true && x.IsDeleted == false && x.CollegeId==applicant.CollegeId).FirstOrDefault();
                        if (qualificationlist != null)
                        {
                            result.Data.HighestQualificationName = qualificationlist.QualName;
                        }
                        if (collegelist != null)
                        {
                            result.Data.CollegeName = collegelist.CollegeName;
                        }

                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = "Please enter a valid Applicant UserId.";
                }
            }
            catch(Exception ex)
            {
                result.Message = "Server error! " + ex.Message;
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteApplicant")]
        public async Task<IActionResult> DeleteApplicant(int applicantId)
        {
            IGeneralResult<ApplicantViewModel> result = new GeneralResult<ApplicantViewModel>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0": user.ClientId.ToString());
                }
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();

                if (applicantId > 0)
                {
                    Applicant applicant = new Applicant();

                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        applicant = _applicantRepo.GetAllQuerable().Where(x => x.ApplicantId == applicantId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        applicant = _applicantRepo.GetAllQuerable().Where(x => x.ApplicantId == applicantId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (applicant == null)
                    {
                        result.Message = " Applicant does Not Exist";
                    }
                    else
                    {
                        applicant.IsActive = false;
                        applicant.IsDeleted = true;
                        applicant.ModifiedDate = DateTime.Now;
                        await _applicantRepo.Update(applicant);

                        result.IsSuccess = true;
                        result.Message = "Applicant deleted successfully.";
                        result.Data = (ApplicantViewModel)applicant;
                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = "Please enter a valid Applicant UserId.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error! " + ex.Message;
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("ApplicantActiveInactive")]
        public async Task<IActionResult> ActiveAndInactive(int applicantId)
        {
            IGeneralResult<ApplicantViewModel> result = new GeneralResult<ApplicantViewModel>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0": user.ClientId.ToString());
                }
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();

                if (applicantId > 0)
                {
                    Applicant applicant = new Applicant();
                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        applicant = _applicantRepo.GetAllQuerable().Where(x => x.ApplicantId == applicantId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        applicant = _applicantRepo.GetAllQuerable().Where(x => x.ApplicantId == applicantId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (applicant == null)
                    {
                        result.Message = " Applicant does Not Exist";
                    }
                    else
                    {
                        applicant.IsActive = !applicant.IsActive;
                        applicant.ModifiedDate = DateTime.Now;
                        await _applicantRepo.Update(applicant);

                        result.IsSuccess = true;
                        result.Message = "Applicant Updated successfully.";
                        result.Data = (ApplicantViewModel)applicant;
                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = "Please enter a valid Applicant UserId.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error! " + ex.Message;
            }
            return Ok(result);
        }
    }
}

