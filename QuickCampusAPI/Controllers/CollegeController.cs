using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CollegeController : ControllerBase
    {
        private readonly ICollegeRepo collegeRepo;
        public CollegeController(ICollegeRepo collegeRepo)
        {
            this.collegeRepo = collegeRepo;  
        }
        [HttpGet]
        [Route("getCollegeManage")]
        public async Task<IActionResult> Manage()
        {
            var model = new CollegeViewModel()
            {
                CollegeList = (await collegeRepo.GetAllCollege()).Select(x => new CollegeGridViewModel()
                {
                    CollegeID = x.CollegeID,
                    CollegeName = x.CollegeName,
                    Address1 = x.Address1,
                    Address2 = x.Address2,
                    City = x.City,
                    StateName = x.StateName,
                    CountryName = x.CountryName,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    ContectPerson = x.ContectPerson,
                    ContectEmail = x.ContectEmail,
                    ContectPhone = x.ContectPhone
                }),
                filter = new CollegeFilter() { },
            };
            return Ok(model);
        }

        //public ActionResult AddCollege(CollegeGridViewModel model, HttpPostedFileBase LogoImage)
        //{
        //    IGeneralResult<dynamic> result = new GeneralResult<dynamic>();
        //    if (ModelState.IsValid)
        //    {
        //        if (LogoImage != null && LogoImage.ContentLength > 0)
        //        {
        //            model.LogoImage = LogoImage.FileName;
        //        }
        //        if (model.CollegeID > 0)
        //        {
        //            result = collegeRepo.UpdateCollege(model);

        //        }
        //        else
        //        {
        //            result = collegeRepo.AddCollege(model);
        //        }
        //        if (result.IsSuccess)
        //        {
        //            #region -- Save College Logo ----
        //            {

        //                if (LogoImage != null && LogoImage.ContentLength > 0)
        //                {
        //                    string physicalPath = QuikCampus.Utility.Common.GenerateFilePath.GetCollegeLogoPath((int)result.Value) + LogoImage.FileName;
        //                    LogoImage.SaveAs(physicalPath);
        //                }
        //            }
        //            #endregion

        //            //result.IsSuccess(result.Message, this);
        //            //return RedirectToAction("Manage");
        //        }
        //        else
        //        {
        //            //NotificationHelper.Alert(result.Message, this);
        //            //return RedirectToAction("Manage");
        //        }
        //    }
        //    else
        //    {
        //    //    NotificationHelper.Success("Please enter required field", this);
        //    //    return RedirectToAction("Manage");
        //    }
        //    return Ok(result);   
        //}

        [HttpGet]
        [Route("activeAndInactive")]
        public async Task<IActionResult> activeAndInactive(bool IsActive, int id)
        {
            IGeneralResult<dynamic> result = new GeneralResult<dynamic>();
            if (id > 0)
            {
                var res = await collegeRepo.GetById(id);
                if (res != null)
                {
                    res.IsActive = IsActive;
                    await collegeRepo.Update(res);
                    result.IsSuccess = true;
                    result.Message = "Your status is changed successfully";
                    result.Data = res;
                    return Ok(result);
                }
                else
                {
                    result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                }
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("collegeDelete")]
        public async Task<IActionResult> Delete(int id)
        {
            IGeneralResult<dynamic> result = new GeneralResult<dynamic>();
            var college = await collegeRepo.GetById(id);

            var res = (college != null && college.IsDeleted == false ) ? college : null;
            if (res != null)
            {
                res.IsActive = false;
                res.IsDeleted = true;
                await collegeRepo.Update(res);
                result.IsSuccess = true;
                result.Message = "Your data is deleted successfully";
                result.Data = res;
                return Ok(result);
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "College Id is not found.";
            }
            return Ok(result);
        }
    }
}
