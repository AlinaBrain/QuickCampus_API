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

        public ApplicantController(IConfiguration configuration, IApplicantRepo applicantRepo)
        {
            _applicantRepo = applicantRepo;
        }

        //[AllowAnonymous]
        [HttpGet]
        [Route("Manage")]
        public async Task<ActionResult> Manage(int statusId)
        {
            try
            {
                IGeneralResult<List<ApplicantViewModel>> result = new GeneralResult<List<ApplicantViewModel>>();


                var rec = await _applicantRepo.GetApplicant();
                var ApplicantList = rec.Where(x => x.StatusId != null).ToList();
                var res = ApplicantList.Select(x => ((ApplicantViewModel)x)).ToList();
                if (res != null)
                {
                    result.IsSuccess = true;
                    result.Message = "List of applicants.";
                    result.Data = res;
                }
                else
                {
                    result.Message = "Applicant not found!";
                }

                return Ok(result);
            }
            catch(Exception ex)
            {

            }
            return Ok();
        }

     [HttpPost]
     [Route("EditApplicant")]
     
       public async Task<IActionResult> EditApplicant(ApplicantRegisterViewModel applicantViewModel, int ApplicantId)
       {
            try
            {
                IGeneralResult<ApplicantViewModel> result = new GeneralResult<ApplicantViewModel>();
                var applicantDetail = _applicantRepo.GetApplicantByID(ApplicantId);
                if (applicantDetail != null)
                {
                    applicantDetail.FirstName = applicantViewModel.FirstName;
                    applicantDetail.LastName = applicantViewModel.LastName;
                    applicantDetail.EmailAddress = applicantViewModel.EmailAddress;
                    applicantDetail.HigestQualification = applicantViewModel.HigestQualification;
                    applicantDetail.MatricPercentage = applicantViewModel.MatricPercentage;
                    applicantDetail.PhoneNumber = applicantViewModel.PhoneNumber;
                    applicantDetail.Skills = applicantViewModel.Skills;
                    _applicantRepo.UpdateApplicant(applicantDetail);
                    result.Message = "Applicant Details updated Succesfully";
                    result.IsSuccess = true;
                }
                else
                {
                    result.Message = "Applicant details does not found";
                }
                return Ok(result);
            }
            catch(Exception ex)
            {

            }
            return Ok();
       }

        [HttpGet]
        [Route("GetApplicantById")]
        public async Task<ActionResult> GetApplicantById(int Id)
        {
            var applicant = _applicantRepo.GetApplicantByID(Id);
            ApplicantRegisterViewModel model = new ApplicantRegisterViewModel()
            {
                ApplicantId = applicant.ApplicantID,
                CollegeId = applicant.CollegeId,
                Colleges = applicant.Colleges,
                EmailAddress = applicant.EmailAddress,
                FirstName = applicant.FirstName,
                HigestQualification = applicant.HigestQualification,
                HigestQualificationPercentage = applicant.HigestQualificationPercentage,
                IntermediatePercentage = applicant.IntermediatePercentage,
                LastName = applicant.LastName,
                MatricPercentage = applicant.MatricPercentage,
                PhoneNumber = applicant.PhoneNumber,
                Skills = applicant.Skills
            };
            return Ok(model);
        }

       
        [HttpDelete]
        [Route("DeleteApplicant")]
        public async Task<IActionResult> DeleteApplicant(int ApplicantId)
        {
            IGeneralResult<ApplicantViewModel> result = new GeneralResult<ApplicantViewModel>();
            var res = await _applicantRepo.GetById(ApplicantId);
            if (res != null)
            {

                await _applicantRepo.Update(res);
                result.Message = "Applicant Details Deleted Succesfully";
            }
            else
            {
                result.Message = "Applicant does Not deleted";
            }
            return Ok(result);
        }
  
    }
}

