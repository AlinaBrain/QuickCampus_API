using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_DAL.Context;

namespace QuickCampusAPI.Controllers
{

    [Authorize(Roles = "Admin,Client,Client_User")]
    [Route("api/[controller]")]
    [ApiController]
    public class HighestQualificationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMstQualificationRepo _qualificationRepo;
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private string _jwtSecretKey;

        public HighestQualificationController(IConfiguration configuration, IMstQualificationRepo qualificationRepo, IUserAppRoleRepo userAppRoleRepo)
        {
            _configuration = configuration;
            _qualificationRepo = qualificationRepo;
            _userAppRoleRepo = userAppRoleRepo;
            _jwtSecretKey = _configuration["Jwt:Key"] ?? "";
        }

        [HttpGet]
        [Route("GetAllQualification")]
        public async Task<ActionResult> GetAllQualifications()                                                                                     
        {
            IGeneralResult<dynamic> result = new GeneralResult<dynamic>();
            try
            {
                var qualificationList = _qualificationRepo.GetAllQuerable().Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                if (qualificationList.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Data fetched successfully.";
                    result.Data = qualificationList.Select(x => new
                    {
                        x.QualId,
                        x.QualName
                    });
                    result.TotalRecordCount = qualificationList.Count;
                }
                else
                {
                    result.Message = "No record found!";
                }
            }
            catch (Exception ex)
            {
                result.Message = "server error! " + ex.Message;
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("AddQualification")]
        public async Task<ActionResult> AddQualification(string QualificationName)
        {
            IGeneralResult<dynamic> result = new GeneralResult<dynamic>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();


                var isQualificationExists = _qualificationRepo.Any(x => x.QualName == QualificationName && x.IsActive == true && x.IsDeleted == false);
                if (!isQualificationExists)
                {
                    MstQualification qualification = new MstQualification
                    {
                        QualName = QualificationName,
                        IsActive = true,
                        IsDeleted = false,
                        CreateAt = DateTime.Now,
                        CreatedBy = Convert.ToInt32(LoggedInUserId)
                    };

                    var QualAdd = await _qualificationRepo.Add(qualification);
                    if (QualAdd.QualId > 0)
                    {
                        result.IsSuccess = true;
                        result.Message = "Data fetched successfully.";
                        result.Data = new { QualAdd.QualId, QualAdd.QualName };
                    }
                    else
                    {
                        result.Message = "Something went wrong.";
                    }
                }
                else
                {
                    result.Message = "Qualification already Exists";
                }
            }
            catch (Exception ex)
            {
                result.Message = "server error! " + ex.Message;
            }
            return Ok(result);
        }

    }
}
