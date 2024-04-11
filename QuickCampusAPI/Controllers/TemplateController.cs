using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Text.RegularExpressions;
using static QuickCampus_Core.Common.common;

namespace QuickCampusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateRepo _templateRepo;
        private readonly IConfiguration _config;
        private readonly string _jwtSecretKey;
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private readonly IUserRepo _userRepo;

        public TemplateController(ITemplateRepo templateRepo, IConfiguration configuration, IUserAppRoleRepo userAppRoleRepo, IUserRepo userRepo)
        {
            _templateRepo = templateRepo;
            _config = configuration;
            _jwtSecretKey = _config["Jwt:Key"] ?? "";
            _userAppRoleRepo = userAppRoleRepo;
            _userRepo = userRepo;
        }
        [HttpGet]
        [Route("GetAllTemplate")]
        public async Task<IActionResult> GetAllTemplate(string? search, int? ClientId, DataTypeFilter DataType, int pageStart = 1, int pageSize = 10)
        {
            IGeneralResult<List<TemplateVm>> result = new GeneralResult<List<TemplateVm>>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                }
                var newPageStart = 0;
                if (pageStart > 0)
                {
                    var startPage = 1;
                    newPageStart = (pageStart - startPage) * pageSize;
                }

                List<TblTemplate> templatelist = new List<TblTemplate>();
                List<TblTemplate> templatedata = new List<TblTemplate>();
                int TemplateListcount = 0;
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    templatedata = _templateRepo.GetAllQuerable().Where(x => (ClientId != null && ClientId > 0 ? x.ClientId == ClientId : true) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                else
                {
                    templatedata = _templateRepo.GetAllQuerable().Where(x => x.ClientId == Convert.ToInt32(LoggedInUserClientId) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim();
                }
                templatelist = templatedata.Where(x => (x.Subject.Contains(search ?? "", StringComparison.OrdinalIgnoreCase) || x.Body.Contains(search ?? "", StringComparison.OrdinalIgnoreCase))).ToList();
                TemplateListcount = templatelist.Count;
                templatelist = templatelist.Skip(newPageStart).Take(pageSize).ToList();

                var response = templatelist.Select(x => (TemplateVm)x).ToList();

                if (templatelist.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Data fetched successfully.";
                    result.Data = response;
                    result.TotalRecordCount = TemplateListcount;
                }
                else
                {
                    result.Message = "Template list not found!";
                }

            }
            catch (Exception ex)
            {
                result.Message = "Server error " + ex.Message;
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("GetTemplateById")]
        public async Task<IActionResult> GetTemplateById(int templateId)
        {
            IGeneralResult<TemplateVm> result = new GeneralResult<TemplateVm>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                }
                if (templateId > 0)
                {
                    TblTemplate template = new TblTemplate();

                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        template = _templateRepo.GetAllQuerable().Where(x => x.Id == templateId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        template = _templateRepo.GetAllQuerable().Where(x => x.Id == templateId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (template == null)
                    {
                        result.Message = " Template does Not Exist";
                    }
                    else
                    {
                        result.IsSuccess = true;
                        result.Message = "Template fetched successfully.";
                        result.Data = (TemplateVm)template;
                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = "Please enter a valid Template  Id.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error! " + ex.Message;
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("AddTemplate")]
        public async Task<IActionResult> AddTemplate(TemplateVm vm)
        {
            IGeneralResult<TemplateVm> result = new GeneralResult<TemplateVm>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                }
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin && (vm.ClientId == null || vm.ClientId.ToString() == "" || vm.ClientId == 0))
                {
                    result.Message = "Please select a valid Client";
                    return Ok(result);
                }
                if (ModelState.IsValid)
                {
                    vm.Body = vm.Body?.Trim();
                    vm.Subject = vm.Subject?.Trim();
                    vm.CreatedBy = Convert.ToInt32(LoggedInUserId);
                    vm.ClientId = (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin) ? vm.ClientId : Convert.ToInt32(LoggedInUserClientId);
                    var SaveApplicant = await _templateRepo.Add(vm.ToTemplateDbModel());
                    if (SaveApplicant.Id > 0)
                    {
                        result.IsSuccess = true;
                        result.Message = "Applicant added successfully.";
                        result.Data = (TemplateVm)SaveApplicant;
                       
                    }
                    else
                    {
                        result.Message = "Template not saved. Please try again.";
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
    }
}