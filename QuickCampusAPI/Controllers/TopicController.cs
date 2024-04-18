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
    public class TopicController : ControllerBase
    {
        private readonly ITopicRepo _topicRepo;
        private readonly IConfiguration _config;
        private readonly string _jwtSecretKey;
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private readonly IUserRepo _userRepo;
        private readonly IDepartmentRepo _departmentRepo;
        private readonly ISubjectRepo _subjectRepo;

        public TopicController(ITopicRepo topicRepo, IConfiguration configuration, IUserAppRoleRepo userAppRoleRepo, IUserRepo userRepo, IDepartmentRepo departmentRepo,ISubjectRepo subjectRepo)
        {
            _topicRepo=topicRepo;
            _config = configuration;
            _jwtSecretKey = _config["Jwt:Key"] ?? "";
            _userAppRoleRepo = userAppRoleRepo;
            _userRepo = userRepo;
            _departmentRepo=departmentRepo;
            _subjectRepo=subjectRepo;
        }
        [HttpGet]
        [Route("GetAllTopic")]
        public async Task<IActionResult> GetAllTopic(string? search, int? ClientId, DataTypeFilter DataType, int pageStart = 1, int pageSize = 10)
        {
            IGeneralResult<List<TopicVm>> result = new GeneralResult<List<TopicVm>>();
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

                List<TblTopic> Topiclist = new List<TblTopic>();
                List<TblTopic> topicdatadata = new List<TblTopic>();
                int TopicListcount = 0;
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    topicdatadata = _topicRepo.GetAllQuerable().Where(x => (ClientId != null && ClientId > 0 ? x.ClientId == ClientId : true) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                else
                {
                    topicdatadata = _topicRepo.GetAllQuerable().Where(x => x.ClientId == Convert.ToInt32(LoggedInUserClientId) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim();
                }
                Topiclist = topicdatadata.Where(x => (x.Name.Contains(search ?? "", StringComparison.OrdinalIgnoreCase))).ToList();
                TopicListcount = Topiclist.Count;
                Topiclist = Topiclist.Skip(newPageStart).Take(pageSize).ToList();

                var response = Topiclist.Select(x => (TopicVm)x).ToList();

                if (Topiclist.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Data fetched successfully.";
                    result.Data = response;
                    result.TotalRecordCount = TopicListcount;
                }
                else
                {
                    result.Message = "Topic list not found!";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error " + ex.Message;
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("GetTopicById")]
        public async Task<IActionResult> GetTopicById(int topicId)
        {
            IGeneralResult<TopicVm> result = new GeneralResult<TopicVm>();
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
                if (topicId > 0)
                {
                    TblTopic topic = new TblTopic();

                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        topic = _topicRepo.GetAllQuerable().Where(x => x.Id == topicId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        topic = _topicRepo.GetAllQuerable().Where(x => x.Id == topicId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (topic == null)
                    {
                        result.Message = " Topic does Not Exist";
                    }
                    else
                    {
                        result.IsSuccess = true;
                        result.Message = "Topic fetched successfully.";
                        result.Data = (TopicVm)topic;
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
        [HttpPost]
        [Route("AddTopic")]
        public async Task<IActionResult> AddTopic(AddTopicVm vm)
        {
            IGeneralResult<AddTopicVm> result = new GeneralResult<AddTopicVm>();
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
                var departmentExist = _departmentRepo.Any(x => x.Id == vm.DepartmentId && x.IsDeleted == false);
                if(!departmentExist)    
                {
                    result.Message = "Department is not exist";
                    return Ok(result);
                }
                var subjectExist= _subjectRepo.Any(x=>x.Id==vm.SubjectId  && x.IsDeleted == false);
                if(!subjectExist)
                {
                    result.Message = "Subject is not exist";
                    return Ok(result);
                }
                if (ModelState.IsValid)
                {
                    var sv = new TblTopic()
                    {
                        Name = vm.Name,
                        IsActive = true,
                        IsDeleted = false,
                        DepartmentId = vm.DepartmentId,
                        SubjectId = vm.SubjectId,
                        CreatedBy = Convert.ToInt32(LoggedInUserId),
                        CreatedDate = DateTime.Now,
                        ClientId = (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin) ? vm.ClientId : Convert.ToInt32(LoggedInUserClientId)
                    };
                    var saveTopic = await _topicRepo.Add(sv);
                    if (saveTopic.Id > 0)
                    {
                        result.IsSuccess = true;
                        result.Message = "Department added successfully.";
                        result.Data = (AddTopicVm)saveTopic;
                    }
                    else
                    {
                        result.Message = "Department not saved. Please try again.";
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
        [Route("UpdateTopic")]
        public async Task<IActionResult> UpdateTopic(EditTopicVm vm)
        {
            IGeneralResult<EditTopicVm> result = new GeneralResult<EditTopicVm>();
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
                var departmentExist = _departmentRepo.Any(x => x.Id == vm.DepartmentId && x.IsDeleted == false);
                if (!departmentExist)
                {
                    result.Message = "Department is not exist";
                    return Ok(result);
                }
                var subjectExist = _subjectRepo.Any(x => x.Id == vm.SubjectId && x.IsDeleted == false);
                if (!subjectExist)
                {
                    result.Message = "Subject is not exist";
                    return Ok(result);
                }
                if (vm.Id > 0)
                {
                    var topic = _topicRepo.GetAllQuerable().Where(x => x.Id == vm.Id && x.IsDeleted == false).FirstOrDefault();
                    if (topic != null)
                    {
                        topic.Name = vm.Name;
                        topic.ModifiedBy = Convert.ToInt32(LoggedInUserId);
                        topic.ModifiedDate = DateTime.Now;
                        topic.SubjectId=vm.SubjectId;
                        topic.DepartmentId = vm.DepartmentId;
                        topic.IsActive = true;
                        topic.IsDeleted = false;
                        topic.ClientId = (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin) ? vm.ClientId : Convert.ToInt32(LoggedInUserClientId);
                        await _topicRepo.Update(topic);
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
        [Route("DeleteTopic")]
        public async Task<IActionResult> DeleteTopic(int topicId)
        {
            IGeneralResult<TopicVm> result = new GeneralResult<TopicVm>();
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
                if (topicId > 0)
                {
                    TblTopic topic = new TblTopic();
                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        topic = _topicRepo.GetAllQuerable().Where(x => x.Id == topicId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        topic = _topicRepo.GetAllQuerable().Where(x => x.Id == topicId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (topic == null)
                    {
                        result.Message = " Topic does Not Exist";
                    }
                    else
                    {
                        topic.IsActive = false;
                        topic.IsDeleted = true;
                        topic.ModifiedDate = DateTime.Now;
                        topic.ModifiedBy = Convert.ToInt32(LoggedInUserId);
                        await _topicRepo.Update(topic);
                        result.IsSuccess = true;
                        result.Message = "Topic deleted successfully.";
                        result.Data = (TopicVm)topic;
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
