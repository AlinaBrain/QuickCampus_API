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
    public class SubTopicController : ControllerBase
    {
        private readonly ISubTopicRepo _subTopicRepo;
        private readonly IUserRepo _userRepo;
        private string _jwtSecretKey;
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private readonly IConfiguration _config;
        private readonly ITopicRepo _topicRepo;

        public SubTopicController(ISubTopicRepo subTopicRepo, IConfiguration configuration, IUserAppRoleRepo userAppRoleRepo, IUserRepo userRepo,ITopicRepo topicRepo)
        {
            _subTopicRepo = subTopicRepo;
            _userRepo=userRepo;
            _config = configuration;
            _topicRepo = topicRepo;
            _userAppRoleRepo = userAppRoleRepo;
            _jwtSecretKey = _config["Jwt:Key"] ?? "";
        }

        [HttpGet]
        [Route("GetAllSubTopic")]
        public async Task<IActionResult> GetAllSubTopic(string? search, int? ClientId, DataTypeFilter DataType, int pageStart = 1, int pageSize = 10)
        {
            IGeneralResult<List<SubTopicVm>> result = new GeneralResult<List<SubTopicVm>>();
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

                List<TblSubTopic> SubTopiclist = new List<TblSubTopic>();
                List<TblSubTopic> SubTopicdatadata = new List<TblSubTopic>();
                int SubTopicListcount = 0;
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    SubTopicdatadata = _subTopicRepo.GetAllQuerable().Where(x => (ClientId != null && ClientId > 0 ? x.ClientId == ClientId : true) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                else
                {
                    SubTopicdatadata = _subTopicRepo.GetAllQuerable().Where(x => x.ClientId == Convert.ToInt32(LoggedInUserClientId) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim();
                }
                SubTopiclist = SubTopicdatadata.Where(x => (x.Name.Contains(search ?? "", StringComparison.OrdinalIgnoreCase))).ToList();
                SubTopicListcount = SubTopiclist.Count;
                SubTopiclist = SubTopiclist.Skip(newPageStart).Take(pageSize).ToList();

                var response = SubTopiclist.Select(x => (SubTopicVm)x).ToList();

                if (SubTopiclist.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Data fetched successfully.";
                    result.Data = response;
                    result.TotalRecordCount = SubTopicListcount;
                }
                else
                {
                    result.Message = "SubTopic list not found!";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error " + ex.Message;
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("GetSubTopicById")]
        public async Task<IActionResult> GetTopicById(int subTopicId)
        {
            IGeneralResult<SubTopicVm> result = new GeneralResult<SubTopicVm>();
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
                if (subTopicId > 0)
                {
                    TblSubTopic subTopic = new TblSubTopic();

                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        subTopic = _subTopicRepo.GetAllQuerable().Where(x => x.Id == subTopicId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        subTopic = _subTopicRepo.GetAllQuerable().Where(x => x.Id == subTopicId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (subTopic == null)
                    {
                        result.Message = " Topic does Not Exist";
                    }
                    else
                    {
                        result.IsSuccess = true;
                        result.Message = "SubTopic fetched successfully.";
                        result.Data = (SubTopicVm)subTopic;
                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = "Please enter a valid SubTopic Id.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error! " + ex.Message;
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("AddSubTopic")]
        public async Task<IActionResult> AddSubTopic(AddSubTopicVm vm)
        {
            IGeneralResult<AddSubTopicVm> result = new GeneralResult<AddSubTopicVm>();
            try
            {
                if (vm == null)
                {
                    result.Message ="Your Model request in Invalid";
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
                var topic = _topicRepo.Any(x => x.Id == vm.TopicId && x.IsDeleted == false);
                if (!topic)
                {
                    result.Message = "SubTopic is not exist";
                    return Ok(result);
                }
               
                if (ModelState.IsValid)
                {
                    var sv = new TblSubTopic()
                    {
                        Name = vm.Name,
                        IsActive = true,
                        IsDeleted = false,
                        TopicId=vm.TopicId,
                        CreatedBy = Convert.ToInt32(LoggedInUserId),
                        CreatedDate = DateTime.Now,
                        ClientId = (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin) ? vm.ClientId : Convert.ToInt32(LoggedInUserClientId)
                    };
                    var saveSubTopic = await _subTopicRepo.Add(sv);
                    if (saveSubTopic.Id > 0)
                    {
                        result.IsSuccess = true;
                        result.Message = "SubTopic added successfully.";
                        result.Data = (AddSubTopicVm)saveSubTopic;
                    }
                    else
                    {
                        result.Message = "SubTopic not saved. Please try again.";
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
        [Route("UpdateSubTopic")]
        public async Task<IActionResult> UpdateSubTopic(EditSubTopicVm vm)
        {
            IGeneralResult<EditSubTopicVm> result = new GeneralResult<EditSubTopicVm>();
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
                var topicExist = _topicRepo.Any(x => x.Id == vm.TopicId && x.IsDeleted == false);
                if (!topicExist)
                {
                    result.Message = "Topic is not exist";
                    return Ok(result);
                }
                
                if (vm.Id > 0)
                {
                    var subTopic = _subTopicRepo.GetAllQuerable().Where(x => x.Id == vm.Id && x.IsDeleted == false).FirstOrDefault();
                    if (subTopic != null)
                    {
                        subTopic.Name = vm.Name;
                        subTopic.ModifiedBy = Convert.ToInt32(LoggedInUserId);
                        subTopic.ModifiedDate = DateTime.Now;
                        subTopic.TopicId = vm.TopicId;
                        subTopic.IsActive = true;
                        subTopic.IsDeleted = false;
                        subTopic.ClientId = (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin) ? vm.ClientId : Convert.ToInt32(LoggedInUserClientId);
                        await _subTopicRepo.Update(subTopic);
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
        [Route("DeleteSubTopic")]
        public async Task<IActionResult> DeleteSubTopic(int subTopicId)
        {
            IGeneralResult<SubTopicVm> result = new GeneralResult<SubTopicVm>();
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
                if (subTopicId > 0)
                {
                     TblSubTopic subTopic= new TblSubTopic();
                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        subTopic = _subTopicRepo.GetAllQuerable().Where(x => x.Id == subTopicId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        subTopic = _subTopicRepo.GetAllQuerable().Where(x => x.Id == subTopicId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (subTopic == null)
                    {
                        result.Message = "SubTopic does Not Exist";
                    }
                    else
                    {
                        subTopic.IsActive = false;
                        subTopic.IsDeleted = true;
                        subTopic.ModifiedDate = DateTime.Now;
                        subTopic.ModifiedBy = Convert.ToInt32(LoggedInUserId);
                        await _subTopicRepo.Update(subTopic);
                        result.IsSuccess = true;
                        result.Message = " SubTopic deleted successfully.";
                        result.Data = (SubTopicVm)subTopic;
                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = "Please enter a valid Topic Id.";
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
