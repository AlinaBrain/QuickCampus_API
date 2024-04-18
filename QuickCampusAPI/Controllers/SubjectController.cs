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
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectRepo _subjectRepo;
        private readonly IConfiguration _config;
        private readonly string _jwtSecretKey;
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private readonly IUserRepo _userRepo;
        private readonly IDepartmentRepo _departmentRepo;

        public SubjectController(ISubjectRepo subjectRepo, IConfiguration configuration, IUserAppRoleRepo userAppRoleRepo, IUserRepo userRepo,IDepartmentRepo departmentRepo)
        {
            _subjectRepo=subjectRepo;
            _config = configuration;
            _jwtSecretKey = _config["Jwt:Key"] ?? "";
            _userAppRoleRepo = userAppRoleRepo;
            _userRepo = userRepo;
            _departmentRepo = departmentRepo;
        }
        [HttpGet]
        [Route("GetAllSubject")]
        public async Task<IActionResult> GetAllSubject(string? search, int? ClientId, DataTypeFilter DataType, int pageStart = 1, int pageSize = 10)
        {
            IGeneralResult<List<SubjectVm>> result = new GeneralResult<List<SubjectVm>>();
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

                List<TblSubject> subjectlist = new List<TblSubject>();
                List<TblSubject> subjectdata = new List<TblSubject>();
                int SubjectListcount = 0;
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    subjectdata = _subjectRepo.GetAllQuerable().Where(x => (ClientId != null && ClientId > 0 ? x.ClientId == ClientId : true) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                else
                {
                    subjectdata = _subjectRepo.GetAllQuerable().Where(x => x.ClientId == Convert.ToInt32(LoggedInUserClientId) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim();
                }
                subjectlist = subjectdata.Where(x => (x.Name.Contains(search ?? "", StringComparison.OrdinalIgnoreCase))).ToList();
                SubjectListcount = subjectlist.Count;
                subjectlist = subjectlist.Skip(newPageStart).Take(pageSize).ToList();

                var response = subjectlist.Select(x => (SubjectVm)x).ToList();

                if (subjectlist.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Data fetched successfully.";
                    result.Data = response;
                    result.TotalRecordCount = SubjectListcount;
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
        [Route("GetSubjectById")]
        public async Task<IActionResult> GetSubjectById(int subjectId)
        {
            IGeneralResult<SubjectVm> result = new GeneralResult<SubjectVm>();
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
                if (subjectId > 0)
                {
                    TblSubject subject = new TblSubject();

                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        subject = _subjectRepo.GetAllQuerable().Where(x => x.Id == subjectId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        subject = _subjectRepo.GetAllQuerable().Where(x => x.Id == subjectId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (subject == null)
                    {
                        result.Message = " Subject does Not Exist";
                    }
                    else
                    {
                        result.IsSuccess = true;
                        result.Message = "Subject fetched successfully.";
                        result.Data = (SubjectVm)subject;
                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = "Please enter a valid Department  Id.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error! " + ex.Message;
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("AddSubject")]
        public async Task<IActionResult> AddSubject(AddSubjectVm vm)
        {
            IGeneralResult<AddSubjectVm> result = new GeneralResult<AddSubjectVm>();
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
                var departmentExist= _departmentRepo.Any(x=>x.Id==vm.DepartmentId && x.IsDeleted==false);
                if (!departmentExist)
                {
                    result.Message = "Department is not exist";
                    return Ok(result);
                }
                if (ModelState.IsValid)
                {
                    var sv = new TblSubject()
                    {
                        Name = vm.Name,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = Convert.ToInt32(LoggedInUserId),
                        CreatedDate = DateTime.Now,
                        DepartmentId=vm.DepartmentId,
                        ClientId = (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin) ? vm.ClientId : Convert.ToInt32(LoggedInUserClientId)
                    };
                    var SaveSubject = await _subjectRepo.Add(sv);
                    if (SaveSubject.Id > 0)
                    {
                        result.IsSuccess = true;
                        result.Message = "Subject added successfully.";
                        result.Data = (AddSubjectVm)SaveSubject;
                    }
                    else
                    {
                        result.Message = "Subject not saved. Please try again.";
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
        [Route("UpdateSubject")]
        public async Task<IActionResult> UpdateSubject(EditSubjectVm vm)
        {
            IGeneralResult<EditSubjectVm> result = new GeneralResult<EditSubjectVm>();
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
                if (vm.Id > 0)
                {

                    var subj = _subjectRepo.GetAllQuerable().Where(x => x.Id == vm.Id && x.IsDeleted == false).FirstOrDefault();
                    if (subj != null)
                    {
                        subj.Name = vm.Name;
                        subj.ModifiedBy = Convert.ToInt32(LoggedInUserId);
                        subj.ModifiedDate = DateTime.Now;
                        subj.IsActive = true;
                        subj.DepartmentId = vm.DepartmentId;
                        subj.IsDeleted = false;
                        subj.ClientId = (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin) ? vm.ClientId : Convert.ToInt32(LoggedInUserClientId);
                        await _subjectRepo.Update(subj);
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
        [Route("DeleteSubject")]
        public async Task<IActionResult> DeleteSubject(int subjectId)
        {
            IGeneralResult<SubjectVm> result = new GeneralResult<SubjectVm>();
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
                if (subjectId > 0)
                {
                    TblSubject subject = new TblSubject();
                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        subject = _subjectRepo.GetAllQuerable().Where(x => x.Id == subjectId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        subject = _subjectRepo.GetAllQuerable().Where(x => x.Id == subjectId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (subject == null)
                    {
                        result.Message = " Subject does Not Exist";
                    }
                    else
                    {
                        subject.IsActive = false;
                        subject.IsDeleted = true;
                        subject.ModifiedDate = DateTime.Now;
                        subject.ModifiedBy = Convert.ToInt32(LoggedInUserId);
                        await _subjectRepo.Update(subject);
                        result.IsSuccess = true;
                        result.Message = "Subject deleted successfully.";
                        result.Data = (SubjectVm)subject;
                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = "Please enter a valid Subject Id.";
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
