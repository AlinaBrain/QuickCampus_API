using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Net.Mail;
using static Azure.Core.HttpHeader;
using static QuickCampus_Core.ViewModel.ApplicantViewModel;

namespace QuickCampusAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ApplicantController : ControllerBase
    {

        private readonly IApplicantRepo _applicantRepo;

        public ApplicantController(IConfiguration configuration, IApplicantRepo applicantRepo)
        {
            _applicantRepo = applicantRepo;
        }
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

     [HttpPut]
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
        //[HttpPost]
        //public JsonResult DeleteMultiple(int[] Ids)
        //{
        //    foreach (int Id in Ids)
        //    {
        //        ApplicantGridViewModel applicant = _applicantRepo.GetApplicantByID(Id);
        //        applicant.StatusID = QuickCampus_Core.Common.(int)common.ApplicantStatus.Deleted;
        //        var result = _applicantRepo.UpdateApplicant(applicant);
        //    }
        //    var model = new ApplicantViewModel()
        //    {
        //        ApplicantList = _applicantRepo.GetAllApplicant().Select(x => new ApplicantGridViewModel()
        //        {
        //            ApplicantID = x.ApplicantID,
        //            ApplicantToken = x.ApplicantToken,
        //            CollegeId = x.CollegeId,
        //            Colleges = x.Colleges,
        //            Comment = x.Comment,
        //            EmailAddress = x.EmailAddress,
        //            FirstName = x.FirstName,
        //            HigestQualification = x.HigestQualification,
        //            HigestQualificationPercentage = x.HigestQualificationPercentage,
        //            IntermediatePercentage = x.IntermediatePercentage,
        //            LastName = x.LastName,
        //            MatricPercentage = x.MatricPercentage,
        //            PhoneNumber = x.PhoneNumber,
        //            RegisteredDate = x.RegisteredDate,
        //            Skills = x.Skills,
        //            StatusID = x.StatusID,
        //            College = ApplicantManager.GetCollegeByApplicantId(x.ApplicantID)
        //        }),
        //        filter = new ApplicantFilter() { },
        //    };
        //    return Ok(new { data = RenderRazorViewToString("_ApplicantGrid", model) });
        //}







        //[HttpPost]
        //[Route("AddApplicant")]
        //public async Task<IActionResult> AddApplicant([FromBody] ApplicantViewModel applicantViewModel)
        //{
        //    IGeneralResult<ApplicantViewModel> result = new GeneralResult<ApplicantViewModel>();
        //    var applicant = await _applicantRepo.Add(applicantViewModel.ToApplicantDbModel());
        //    if (applicant.ApplicantId != 0)
        //    {
        //        result.IsSuccess = true;
        //        result.Message = "Category Added Successfully";
        //    }
        //    else
        //    {
        //        result.Message = "something Went Wrong";
        //    }
        //    return Ok(result);
        //}


        
    }
}

