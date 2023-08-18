using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using Microsoft.AspNetCore.Http;
using QuickCampus_DAL.Context;
using QuickCampus_Core.Services;
using Azure;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using System;

namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CollegeController : ControllerBase
    {
        private readonly ICollegeRepo _collegeRepo;
        private IConfiguration _config;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly string basepath;
        private readonly ICountryRepo _countryRepo;
        private readonly IStateRepo _stateRepo;
        private string baseUrl;
        public CollegeController(ICollegeRepo collegeRepo, IConfiguration config, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, ICountryRepo countryRepo, IStateRepo stateRepo)
        {
            _collegeRepo = collegeRepo;
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            baseUrl = _config.GetSection("APISitePath").Value;
            _countryRepo = countryRepo;
            _stateRepo = stateRepo;

        }

        [Authorize(Roles = "GetAllCollege")]
        [HttpGet]
        [Route("GetAllCollege")]
        public async Task<IActionResult> GetAllCollege(int clientid,int pageStart=0,int pageSize=10)
        {
            IGeneralResult<List<GetCollegeVm>> result = new GeneralResult<List<GetCollegeVm>>();
            var _jwtSecretKey = _config["Jwt:Key"];

            int cid = 0;
            var jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
            }
            List<College> collegeList = new List<College>();
            try
            {
                var collegeListCount=0;

                if (isSuperAdmin)
                {
                   collegeListCount = (await _collegeRepo.GetAll()).Where(x => x.IsDeleted != true && (cid == 0 ? true : x.ClientId == cid)).Count();
                   collegeList = (List<College>)(await _collegeRepo.GetAll()).Where(x => x.IsDeleted != true && (cid == 0 ? true : x.ClientId == cid)).Skip(pageStart).Take(pageSize).OrderByDescending(x=>x.CollegeId).ToList();

                }
                else
                {
                    collegeListCount = (await _collegeRepo.GetAll()).Where(x => x.IsDeleted != true && x.ClientId == cid).Count();
                    collegeList = (List<College>)(await _collegeRepo.GetAll()).Where(x => x.IsDeleted != true && x.ClientId == cid).Skip(pageStart).Take(pageSize).OrderByDescending(x=>x.CollegeId).ToList();
                }

                var response = collegeList.Select(x => (GetCollegeVm)x).OrderByDescending(x => x.CollegeId).ToList();

                
                if (collegeList.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "College get successfully";
                    result.Data = response;
                    result.TotalRecordCount = collegeListCount;
                }
                else
                {
                    result.IsSuccess = true;
                    result.Message = "College list not found!";
                    result.Data = null;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }

        [Authorize(Roles = "GetCollegeDetailsById")]
        [HttpGet]
        [Route("GetCollegeDetailsById")]
        public async Task<IActionResult> GetCollegeDetailsById(int Id, int clientid)
        {
            IGeneralResult<GetCollegeVm> result = new GeneralResult<GetCollegeVm>();
            var _jwtSecretKey = _config["Jwt:Key"];
            int cid = 0;
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
            }

            var res = await _collegeRepo.GetById(Id);
            if (res.IsDeleted == false && res.IsActive == true)
            {
                result.Data = (GetCollegeVm)res;
                result.IsSuccess = true;
                result.Message = "College details getting succesfully";
            }
            else
            {
                result.Message = "College does Not exist";
            }
            return Ok(result);
        }

        [Authorize(Roles = "AddCollege")]
        [HttpPost]
        [Route("AddCollege")]
        public async Task<IActionResult> AddCollege([FromForm] CollegeLogoVm vm, int clientid)
        {
            IGeneralResult<CollegeVM> result = new GeneralResult<CollegeVM>();
            var _jwtSecretKey = _config["Jwt:Key"];
            int cid = 0;
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
            }
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (vm != null)
            {

                if (ModelState.IsValid)
                {
                    bool isExits = _collegeRepo.Any(x => x.CollegeName == vm.CollegeName && x.IsDeleted == false);
                    if (isExits)
                    {
                        result.Message = " CollegeName is already exists";
                    }
                    bool isexist = _collegeRepo.Any(x => x.CollegeCode == vm.CollegeCode && x.IsDeleted == false);
                    if (isexist)
                    {
                        result.Message = "CollegeCode is alredy exist";
                    }
                    bool iscontactemail = _collegeRepo.Any(x => x.ContectEmail == vm.ContectEmail && x.IsDeleted == false);
                    if (iscontactemail)
                    {
                        result.Message = "Contact Email is Already Exist";
                    }
                    bool iscontactperson = _collegeRepo.Any(x => x.ContectPerson == vm.ContectPerson && x.IsDeleted == false);
                    if (iscontactperson)
                    {
                        result.Message = "Contact Person is Already Exist";
                    }
                    else
                    {
                        var Countrylist = await _countryRepo.GetAll(x => x.CountryId == vm.CountryId);
                        {
                          
                            CollegeVM college = new CollegeVM
                            {
                                CollegeName = vm.CollegeName.Trim(),
                                Logo = ProcessUploadFile(vm),
                                Address1 = vm.Address1.Trim(),
                                Address2 = vm.Address2,
                                CreatedBy = Convert.ToInt32(userId),
                                ModifiedBy = Convert.ToInt32(userId),
                                City = vm.City.Trim(),
                                StateId = vm.StateId,
                                CountryId = vm.CountryId,
                                CollegeCode = vm.CollegeCode,
                                ContectPerson = vm.ContectPerson.Trim(),
                                ContectEmail = vm.ContectEmail.Trim(),
                                ContectPhone = vm.ContectPhone.Trim(),
                                ClientId = cid,

                            };
                            try
                            {
                                var collegedata = await _collegeRepo.Add(college.ToCollegeDbModel());
                                result.Data = (CollegeVM)collegedata;
                                result.Message = "College added successfully";
                                result.IsSuccess = true;
                            }

                            catch (Exception ex)
                            {
                                result.Message = ex.Message;
                            }
                            return Ok(result);
                        }
                    }
                }
                else
                {
                    result.Message = "something Went Wrong";
                }
            }
            return Ok(result);
        }

        [Authorize(Roles = "EditCollege")]
        [HttpPost]
        [Route("EditCollege")]
        public async Task<IActionResult> EditCollege([FromForm] CollegeLogoVm vm, int clientid)
        {
            IGeneralResult<CollegeLogoVm> result = new GeneralResult<CollegeLogoVm>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);

            int cid = 0;
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
                if (cid == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "Invalid Client";
                    return Ok(result);
                }
            }
            if (vm != null)
            {
                College clg = new College();

                if (isSuperAdmin)
                {
                    clg = (await _collegeRepo.GetAll()).Where(w => w.IsDeleted == false && w.IsActive == true && (cid == 0 ? true : w.ClientId == cid )&& w.CollegeId == vm.CollegeId).FirstOrDefault();
                }
                else
                {
                    clg = (await _collegeRepo.GetAll()).Where(w => w.IsDeleted == false && w.IsActive == true && w.ClientId == cid).FirstOrDefault();
                }
                if (clg == null)
                {
                    result.IsSuccess = false;
                    result.Message = " College does Not Exist";
                    return Ok(result);
                }
                bool isDeleted = (bool)clg.IsDeleted ? true : false;
                if (isDeleted)
                {
                    result.IsSuccess = false;
                    result.Message = " College does Not Exist";
                    return Ok(result);
                }
                bool isExits = _collegeRepo.Any(x => x.CollegeName == vm.CollegeName && x.IsDeleted == false && x.CollegeId!=vm.CollegeId);
                if (isExits)
                {
                    result.Message = " CollegeName is already exists";
                    return Ok(result);
                }
                bool isexist = _collegeRepo.Any(x => x.CollegeCode == vm.CollegeCode && x.IsDeleted == false && x.CollegeId != vm.CollegeId);
                if (isexist)
                {
                    result.Message = "CollegeCode is alredy exist";
                    return Ok(result);
                }
                bool iscontactemail = _collegeRepo.Any(x => x.ContectEmail == vm.ContectEmail && x.IsDeleted == false && x.CollegeId != vm.CollegeId);
                if (iscontactemail)
                {
                    result.Message = "Contact Email is Already Exist";
                    return Ok(result);
                }
                bool iscontactperson = _collegeRepo.Any(x => x.ContectPerson == vm.ContectPerson && x.IsDeleted == false && x.CollegeId != vm.CollegeId);
                if (iscontactperson)
                {
                    result.Message = "Contact Person is Already Exist";
                    return Ok(result);
                }
                else
                {
                    
                    if (ModelState.IsValid && vm.CollegeId > 0)
                    {
                       
                        clg.CollegeId = vm.CollegeId;
                        clg.CollegeName = vm.CollegeName.Trim();
                        clg.Logo =  vm.ImagePath !=null? ProcessUploadFile(vm) : clg.Logo;
                        clg.Address1 = vm.Address1.Trim();
                        clg.Address2 = string.IsNullOrEmpty(vm.Address2)?clg.Address2.Trim():vm.Address2.Trim();
                        clg.CreatedBy = Convert.ToInt32(userId);
                        clg.ModifiedBy = Convert.ToInt32(userId);
                        clg.City = vm.City.Trim();
                        clg.StateId = vm.StateId;
                        clg.CountryId = vm.CountryId;
                        clg.CollegeCode = vm.CollegeCode.Trim();
                        clg.ContectPerson = vm.ContectPerson.Trim();
                        clg.ContectEmail = vm.ContectEmail.Trim();
                        clg.ContectPhone = vm.ContectPhone.Trim();
                        clg.ModifiedDate = DateTime.Now;

                        try
                        {
                            result.Data = (CollegeLogoVm)await _collegeRepo.Update(clg);
                            result.Message = "College updated successfully";
                            result.IsSuccess = true;
                        }
                        catch (Exception ex)
                        {
                            result.Message = ex.Message;
                        }
                        return Ok(result);
                    }
                    else
                    {
                        result.Message = "something Went Wrong";
                    }
                }
            }
            return Ok(result);
        }

        [Authorize(Roles = "DeleteCollege")]
        [HttpDelete]
        [Route("DeleteCollege")]
        public async Task<IActionResult> DeleteCollege(int id, int clientid,bool isDeleted)
        {
            IGeneralResult<CollegeVM> result = new GeneralResult<CollegeVM>();
            int cid = 0;
            var jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);

                if (cid == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "Invalid College";
                    return Ok(result);
                }
            }
            var res =await _collegeRepo.DeleteCollege(isDeleted, id, cid, isSuperAdmin);
            return Ok(res);
        }

        [Authorize(Roles = "ActiveAndInactive")]
        [HttpGet]
        [Route("ActiveAndInactive")]
        public async Task<IActionResult> ActiveAndInactive(bool isActive, int id)
        {
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            IGeneralResult<CollegeVM> result = new GeneralResult<CollegeVM>();
            var res = await _collegeRepo.GetById(id);
            if (res.IsDeleted == false)
            {

                res.IsActive = isActive;
                res.IsDeleted = false;
                var data = await _collegeRepo.Update(res);
                result.Data = (CollegeVM)data;
                result.IsSuccess = true;
                result.Message = "College status changed succesfully";
            }
            else
            {
                result.Message = "College does Not exist";
            }
            return Ok(result);
        }
        private string ProcessUploadFile([FromForm] CollegeLogoVm model)
        {
             List<string> url = new List<string>();
            string uniqueFileName = null;
            if (model.ImagePath != null)
            {
                string photoUoload = Path.Combine(_hostingEnvironment.WebRootPath, "UploadFiles");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
                string filepath = Path.Combine(photoUoload, uniqueFileName);
                using (var filename = new FileStream(filepath, FileMode.Create))
                {
                    model.ImagePath.CopyTo(filename);
                }
            }

            url.Add(Path.Combine(baseUrl, uniqueFileName));
            return url.FirstOrDefault();
        }

        private CountryVM GetCountryDetails(int countryId)
        {
            CountryVM countryVm = new CountryVM();

            var countryDetails = _countryRepo.GetById(countryId).Result;
            countryVm.CountryId = countryDetails.CountryId;
            countryVm.CountryName = countryDetails.CountryName;
            return countryVm;
        }

        private StateVM GetstateDetails(int stateId)
        {
            StateVM statevm= new StateVM();

            var stateDetails =_stateRepo.GetById(stateId).Result;
            statevm.CountryId = stateDetails.CountryId;
            statevm.StateName = stateDetails.StateName;
            statevm.StateId = stateDetails.StateId; 
            return statevm;
        }
        [HttpPost]
        [Route("ProcessUploadFile")]
        public List<string>ProcessUploadFile(List<IFormFile> Files)
        {
            List<string> url = new List<string>();
            if (Files.Count > 0)
            {
                foreach (IFormFile file in Files)
                {
                    string uniqueFileName = null;
                    string photoUpload = Path.Combine(_hostingEnvironment.WebRootPath, "UploadFiles");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filepath = Path.Combine(photoUpload, uniqueFileName);
                    using (var filename = new FileStream(filepath, FileMode.Create))
                    {
                        file.CopyTo(filename);
                    }
                    url.Add(Path.Combine(basepath, uniqueFileName));
                }
                return url;
            }
            else
            {
                url.Add("Please add atleast one file.");
                return url;
            }
        }
        [Authorize(Roles = "GetAllActiveCollege")]
        [HttpGet]
        [Route("GetAllActiveCollege")]
        public async Task<IActionResult> GetAllActiveCollege(int clientid)
        {
            IGeneralResult<List<CollegeVM>> result = new GeneralResult<List<CollegeVM>>();
            int cid = 0;
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
            }
            try
            {

                var collegeListCount = (await _collegeRepo.GetAll()).Where(x => x.IsActive == true && (cid == 0 ? true : x.CollegeId== cid)).Count();
                var collegetList = (await _collegeRepo.GetAll()).Where(x => x.IsActive == true && (cid == 0 ? true : x.CollegeId == cid)).OrderByDescending(x => x.CollegeId).ToList();

                var res = collegetList.Select(x => ((CollegeVM)x)).ToList();
                if (res != null && res.Count() > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "ActiveCollegeList";
                    result.Data = res;
                    result.TotalRecordCount = collegeListCount;
                }
                else
                {
                    result.Message = " Active College List Not Found";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }
    }
}


