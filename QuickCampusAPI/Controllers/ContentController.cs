using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using static QuickCampus_Core.Common.common;

namespace QuickCampusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentRepo _contentRepo;
        private readonly IConfiguration _config;
        private readonly string _jwtSecretKey;
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private readonly IUserRepo _userRepo;
        private readonly IMstContentTypeRepo _mstContentTypeRepo;

        public ContentController(IContentRepo contentRepo , IConfiguration configuration, IUserAppRoleRepo userAppRoleRepo, IUserRepo userRepo,IMstContentTypeRepo mstContentTypeRepo)
        {
            _contentRepo = contentRepo;
            _config = configuration;
            _jwtSecretKey = _config["Jwt:Key"] ?? "";
            _userAppRoleRepo = userAppRoleRepo;
            _userRepo = userRepo;
            _mstContentTypeRepo = mstContentTypeRepo;
        }
        [HttpGet]
        [Route("GetAllContent")]
        public async Task<IActionResult> GetAllContent(string? search, int? ClientId, DataTypeFilter DataType, int pageStart = 1, int pageSize = 10)
        {
            IGeneralResult<List<TblContentVm>> result = new GeneralResult<List<TblContentVm>>();
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

                List<TblContent> Contentlist = new List<TblContent>();
                List<TblContent> Contentdata = new List<TblContent>();
                int ContentListcount = 0;
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    Contentdata = _contentRepo.GetAllQuerable().Where(x => (ClientId != null && ClientId > 0 ? x.ClientId == ClientId : true) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                else
                {
                    Contentdata = _contentRepo.GetAllQuerable().Where(x => x.ClientId == Convert.ToInt32(LoggedInUserClientId) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim();
                }
                Contentlist = Contentdata.Where(x => (x.Content.Contains(search ?? "", StringComparison.OrdinalIgnoreCase))).ToList();
                ContentListcount = Contentlist.Count;
                Contentlist = Contentlist.Skip(newPageStart).Take(pageSize).ToList();

                var response = Contentlist.Select(x => (TblContentVm)x).ToList();

                if (Contentlist.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Data fetched successfully.";
                    result.Data = response;
                    result.TotalRecordCount = ContentListcount;
                }
                else
                {
                    result.Message = "Content list not found!";
                }

            }
            catch (Exception ex)
            {
                result.Message = "Server error " + ex.Message;
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetContentById")]
        public async Task<IActionResult> GetContentById(int contentId)
        {
            IGeneralResult<TblContentVm> result = new GeneralResult<TblContentVm>();
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
                if (contentId > 0)
                {
                    TblContent content = new TblContent();

                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        content = _contentRepo.GetAllQuerable().Where(x => x.Id == contentId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        content = _contentRepo.GetAllQuerable().Where(x => x.Id == contentId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (content == null)
                    {
                        result.Message = " Content does Not Exist";
                    }
                    else
                    {
                        result.IsSuccess = true;
                        result.Message = "Department fetched successfully.";
                        result.Data = (TblContentVm)content;
                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = "Please enter a valid Content Id.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error! " + ex.Message;
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("AddContent")]
        public async Task<IActionResult> AddContent(AddContentVm vm)
        {
            IGeneralResult<AddContentVm> result = new GeneralResult<AddContentVm>();
            try
            {
                if (vm == null)
                {
                    result.Message = "Your Model request in Invalid";
                    return Ok(result);
                }
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
                var MstContentExist = _mstContentTypeRepo.Any(x => x.Id == vm.ContentTypeId && x.IsDeleted == false);
                if (!MstContentExist)
                {
                    result.Message = "MstContent is not exist";
                    return Ok(result);
                }
                if (ModelState.IsValid)
                {
                    var sv = new TblContent()
                    {
                        Content = vm.Content,
                        IsActive = true,
                        IsDeleted = false,
                        ContentTypeId = vm.ContentTypeId,
                        CreatedDate = DateTime.Now,
                        ClientId = (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin) ? vm.ClientId : Convert.ToInt32(LoggedInUserClientId)
                    };
                    var SaveContent = await _contentRepo.Add(sv);
                    if (SaveContent.Id > 0)
                    {
                        result.IsSuccess = true;
                        result.Message = "Content added successfully.";
                        result.Data = (AddContentVm)SaveContent;
                    }
                    else
                    {
                        result.Message = "Content not saved. Please try again.";
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
        [Route("UpdateContent")]
        public async Task<IActionResult> UpdateContent(EditContentVm vm)
        {
            IGeneralResult<EditContentVm> result = new GeneralResult<EditContentVm>();
            try
            {
                if (vm == null)
                {
                    result.Message = "Your Model request in Invalid";
                    return Ok(result);
                }
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
                if (vm.Id > 0)
                {
                    var content = _contentRepo.GetAllQuerable().Where(x => x.Id == vm.Id && x.IsDeleted == false).FirstOrDefault();
                    if (content != null)
                    {
                        content.Content = vm.Content;
                        content.ContentTypeId =vm.ContentTypeId;
                        content.UpdatedDate = DateTime.Now;
                        content.ClientId = (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin) ? vm.ClientId : Convert.ToInt32(LoggedInUserClientId);
                        await _contentRepo.Update(content);
                        result.IsSuccess = true;
                        result.Message = "Record Update Successfully";
                        result.Data = vm;
                        return Ok(result);
                    }
                }
                else
                {
                    result.Message = "Something went wrong.";
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                result.Message = "server error. " + ex.Message;
            }
            return Ok(result);
        }
        [HttpDelete]
        [Route("DeleteContent")]
        public async Task<IActionResult> DeleteContent(int contentid)
        {
            IGeneralResult<TblContentVm> result = new GeneralResult<TblContentVm>();
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
                if (contentid > 0)
                {
                    TblContent content = new TblContent();
                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        content = _contentRepo.GetAllQuerable().Where(x => x.Id == contentid && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        content = _contentRepo.GetAllQuerable().Where(x => x.Id == contentid && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (content == null)
                    {
                        result.Message = " content does Not Exist";
                    }
                    else
                    {
                        content.IsActive = false;
                        content.IsDeleted = true;
                        content.UpdatedDate = DateTime.Now;
                      
                        await _contentRepo.Update(content);
                        result.IsSuccess = true;
                        result.Message = "Content deleted successfully.";
                        result.Data = (TblContentVm)content;
                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = "Please enter a valid Content Id.";
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
