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
    public class TagController : ControllerBase
    {
        private readonly ITagRepo _tagRepo;
        private readonly IUserRepo _userRepo;
        private readonly IConfiguration _config;
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private string _jwtSecretKey;
        public TagController(ITagRepo tagRepo, IConfiguration configuration, IUserAppRoleRepo userAppRoleRepo, IUserRepo userRepo)
        {
            _tagRepo = tagRepo;
            _userRepo = userRepo;
            _config = configuration;
            _userAppRoleRepo = userAppRoleRepo;
            _jwtSecretKey = _config["Jwt:Key"] ?? "";
        }
        [HttpGet]
        [Route("GetAllTag")]
        public async Task<IActionResult> GetAllTag(string? search, int? ClientId, DataTypeFilter DataType, int pageStart = 1, int pageSize = 10)
        {
            IGeneralResult<List<TagVm>> result = new GeneralResult<List<TagVm>>();
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

                List<TblTag> Taglist = new List<TblTag>();
                List<TblTag> Tagdata = new List<TblTag>();
                int TagListcount = 0;
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    Tagdata = _tagRepo.GetAllQuerable().Where(x => (ClientId != null && ClientId > 0 ? x.ClientId == ClientId : true) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                else
                {
                    Tagdata = _tagRepo.GetAllQuerable().Where(x => x.ClientId == Convert.ToInt32(LoggedInUserClientId) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim();
                }
                Taglist = Tagdata.Where(x => (x.Name.Contains(search ?? "", StringComparison.OrdinalIgnoreCase))).ToList();
                TagListcount = Taglist.Count;
                Taglist = Taglist.Skip(newPageStart).Take(pageSize).ToList();

                var response = Taglist.Select(x => (TagVm)x).ToList();

                if (Taglist.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Data fetched successfully.";
                    result.Data = response;
                    result.TotalRecordCount = TagListcount;
                }
                else
                {
                    result.Message = "Tag list not found!";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error " + ex.Message;
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("GetTagById")]
        public async Task<IActionResult> GetTagById(int tagId)
        {
            IGeneralResult<TagVm> result = new GeneralResult<TagVm>();
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
                if (tagId > 0)
                {
                    TblTag tag = new TblTag();

                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        tag = _tagRepo.GetAllQuerable().Where(x => x.Id == tagId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        tag = _tagRepo.GetAllQuerable().Where(x => x.Id == tagId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (tag == null)
                    {
                        result.Message = " Tag does Not Exist";
                    }
                    else
                    {
                        result.IsSuccess = true;
                        result.Message = "Tag fetched successfully.";
                        result.Data = (TagVm)tag;
                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = "Please enter a valid Tag Id.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error! " + ex.Message;
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("AddTag")]
        public async Task<IActionResult> AddTag(AddTagVm vm)
        {
            IGeneralResult<AddTagVm> result = new GeneralResult<AddTagVm>();
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
                if (ModelState.IsValid)
                {
                    var sv = new TblTag()
                    {
                        Name = vm.Name,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = Convert.ToInt32(LoggedInUserId),
                        CreatedDate = DateTime.Now,
                        ClientId = (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin) ? vm.ClientId : Convert.ToInt32(LoggedInUserClientId)
                    };
                    var saveTag = await _tagRepo.Add(sv);
                    if (saveTag.Id > 0)
                    {
                        result.IsSuccess = true;
                        result.Message = "Tag added successfully.";
                        result.Data = (AddTagVm)saveTag;
                    }
                    else
                    {
                        result.Message = "Tag not saved. Please try again.";
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
        [Route("UpdateTag")]
        public async Task<IActionResult> UpdateTag(EditTagVm vm)
        {
            IGeneralResult<EditTagVm> result = new GeneralResult<EditTagVm>();
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
                    var tag = _tagRepo.GetAllQuerable().Where(x => x.Id == vm.Id && x.IsDeleted == false).FirstOrDefault();
                    if (tag != null)
                    {
                        tag.Name = vm.Name;
                        tag.ModifiedBy = Convert.ToInt32(LoggedInUserId);
                        tag.ModifiedDate = DateTime.Now;

                        tag.IsActive = true;
                        tag.IsDeleted = false;
                        tag.ClientId = (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin) ? vm.ClientId : Convert.ToInt32(LoggedInUserClientId);
                        await _tagRepo.Update(tag);
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
        [Route("DeleteTag")]
        public async Task<IActionResult> DeleteTag(int tagId)
        {
            IGeneralResult<TagVm> result = new GeneralResult<TagVm>();
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
                if (tagId > 0)
                {
                    TblTag tblTag = new TblTag();
                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        tblTag = _tagRepo.GetAllQuerable().Where(x => x.Id == tagId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        tblTag = _tagRepo.GetAllQuerable().Where(x => x.Id == tagId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (tblTag == null)
                    {
                        result.Message = "SubTopic does Not Exist";
                    }
                    else
                    {
                        tblTag.IsActive = false;
                        tblTag.IsDeleted = true;
                        tblTag.ModifiedDate = DateTime.Now;
                        tblTag.ModifiedBy = Convert.ToInt32(LoggedInUserId);
                        await _tagRepo.Update(tblTag);
                        result.IsSuccess = true;
                        result.Message = " Tag deleted successfully.";
                        result.Data = (TagVm)tblTag;
                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = "Please enter a valid Tag Id.";
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
