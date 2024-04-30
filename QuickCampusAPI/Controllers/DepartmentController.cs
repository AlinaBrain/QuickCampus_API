using Microsoft.AspNetCore.Authorization;
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
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepo _departmentRepo;
        private readonly IConfiguration _config;
        private readonly string _jwtSecretKey;
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private readonly IUserRepo _userRepo;
        public DepartmentController(IDepartmentRepo departmentRepo, IConfiguration configuration, IUserAppRoleRepo userAppRoleRepo, IUserRepo userRepo)
        {
            _departmentRepo= departmentRepo;
            _config = configuration;
            _jwtSecretKey = _config["Jwt:Key"] ?? "";
            _userAppRoleRepo = userAppRoleRepo;
            _userRepo = userRepo;
        }
        [HttpGet]
        [Route("GetAllDepartment")]
        public async Task<IActionResult> GetAllDepartment(string? search, int? ClientId, DataTypeFilter DataType, int pageStart = 1, int pageSize = 10)
        {
            IGeneralResult<List<DepartmentVm>> result = new GeneralResult<List<DepartmentVm>>();
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

                List<TblDepartment> departmentlist = new List<TblDepartment>();
                List<TblDepartment> departmentdata = new List<TblDepartment>();
                int DepartmentListcount = 0;
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    departmentdata = _departmentRepo.GetAllQuerable().Where(x => (ClientId != null && ClientId > 0 ? x.ClientId == ClientId : true) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                else
                {
                    departmentdata = _departmentRepo.GetAllQuerable().Where(x => x.ClientId == Convert.ToInt32(LoggedInUserClientId) && x.IsDeleted == false && ((DataType == DataTypeFilter.OnlyActive ? x.IsActive == true : (DataType == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)))).ToList();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim();
                }
                departmentlist = departmentdata.Where(x => (x.Name.Contains(search ?? "", StringComparison.OrdinalIgnoreCase))).ToList();
                DepartmentListcount = departmentlist.Count;
                departmentlist = departmentlist.Skip(newPageStart).Take(pageSize).ToList();

                var response = departmentlist.Select(x => (DepartmentVm)x).ToList();

                if (departmentlist.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Data fetched successfully.";
                    result.Data = response;
                    result.TotalRecordCount = DepartmentListcount;
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
        [Route("GetDepartmentById")]
        public async Task<IActionResult> GetDepartmentById(int departmentId)
        {
            IGeneralResult<DepartmentVm> result = new GeneralResult<DepartmentVm>();
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
                if (departmentId > 0)
                {
                    TblDepartment department = new TblDepartment();

                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        department = _departmentRepo.GetAllQuerable().Where(x => x.Id == departmentId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        department = _departmentRepo.GetAllQuerable().Where(x => x.Id == departmentId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (department == null)
                    {
                        result.Message = " Department does Not Exist";
                    }
                    else
                    {
                        result.IsSuccess = true;
                        result.Message = "Department fetched successfully.";
                        result.Data = (DepartmentVm)department;
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
        [Route("AddDepartment")]
        public async Task<IActionResult> AddDepartment(AddDepartmentVm vm)
        {
            IGeneralResult<AddDepartmentVm> result = new GeneralResult<AddDepartmentVm>();
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
                    var sv = new TblDepartment()
                    {
                        Name=vm.Name,
                        IsActive=true,
                        IsDeleted=false,
                        CreatedBy= Convert.ToInt32(LoggedInUserId),
                        CreatedDate= DateTime.Now,
                        ClientId = (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin) ? vm.ClientId : Convert.ToInt32(LoggedInUserClientId)
                    };
                    var SaveDepartment = await _departmentRepo.Add(sv);
                    if (SaveDepartment.Id > 0)
                    {
                        result.IsSuccess = true;
                        result.Message = "Department added successfully.";
                        result.Data = (AddDepartmentVm)SaveDepartment;
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
        [Route("UpdateDepartment")]
        public async Task<IActionResult> UpdateDepartment(DepartmentVm vm)
        {
            IGeneralResult<DepartmentVm> result = new GeneralResult<DepartmentVm>();
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
                    var dept = _departmentRepo.GetAllQuerable().Where(x => x.Id == vm.Id && x.IsDeleted == false).FirstOrDefault();
                    if (dept != null)
                    {
                        dept.Name = vm.Name;
                        dept.ModifiedBy =Convert.ToInt32(LoggedInUserId);
                        dept.ModifiedDate = DateTime.Now;
                        dept.ClientId = (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin) ? vm.ClientId : Convert.ToInt32(LoggedInUserClientId);
                        await _departmentRepo.Update(dept);
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
        [Route("DeleteDepartment")]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {
            IGeneralResult<DepartmentVm> result = new GeneralResult<DepartmentVm>();
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
                if (departmentId > 0)
                {
                    TblDepartment department = new TblDepartment();
                    if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                    {
                        department = _departmentRepo.GetAllQuerable().Where(x => x.Id == departmentId && x.IsDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        department = _departmentRepo.GetAllQuerable().Where(x => x.Id == departmentId && x.IsDeleted == false && x.ClientId == Convert.ToInt32(LoggedInUserClientId)).FirstOrDefault();
                    }
                    if (department == null)
                    {
                        result.Message = " TblCollege does Not Exist";
                    }
                    else
                    {
                        department.IsActive = false;
                        department.IsDeleted = true;
                        department.ModifiedDate = DateTime.Now;
                        department.ModifiedBy = Convert.ToInt32(LoggedInUserId);
                        await _departmentRepo.Update(department);
                        result.IsSuccess = true;
                        result.Message = "Department deleted successfully.";
                        result.Data = (DepartmentVm)department;
                    }
                    return Ok(result);
                }
                else
                {
                    result.Message = "Please enter a valid Department Id.";
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
    

