using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using Microsoft.AspNetCore.Http;
using QuickCampus_DAL.Context;
using QuickCampus_Core.Services;
using Azure;

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
        public CollegeController(ICollegeRepo collegeRepo, IConfiguration config, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,ICountryRepo countryRepo,IStateRepo stateRepo)
        {
            _collegeRepo = collegeRepo;
            _config = config;
            _hostingEnvironment=hostingEnvironment;
            basepath = config["APISitePath"];
            _countryRepo = countryRepo;
            _stateRepo=stateRepo;

        }

        [Authorize(Roles = "GetAllCollege")]
        [HttpGet]
        [Route("GetAllCollege")]
        public async Task<IActionResult> GetAllCollege(int clientid)
        {
            IGeneralResult<List<CollegeVM>> result = new GeneralResult<List<CollegeVM>>();
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

                if (isSuperAdmin)
                {
                    collegeList = (await _collegeRepo.GetAll()).Where(x => x.IsDeleted != true && (cid == 0 ? true : x.ClientId == cid)).ToList();

                }
                else
                {
                    collegeList = (await _collegeRepo.GetAll()).Where(x => x.IsDeleted != true && x.ClientId == cid).ToList();
                }

               var response = collegeList.Select(x => (CollegeVM)x).ToList();


                if (collegeList.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "College get successfully";
                    result.Data = response;
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

            var res = await _collegeRepo.GetById(Id);
            if (res.IsDeleted == false && res.IsActive == true)
            {
                result.Data = (CollegeVM)res;
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
        public async Task<IActionResult> AddCollege([FromForm] CollegeLogoVm vm,int clientid)
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
                    bool isexist = _collegeRepo.Any(x => x.CollegeCode == vm.CollegeCode && x.IsDeleted==false);
                    if (isexist)
                    {
                        result.Message = "CollegeCode is alredy exist";
                    }

                    else
                    {
                        {
                            CollegeVM college = new CollegeVM
                            {
                                CollegeName = vm.CollegeName.Trim(),
                                Logo = ProcessUploadFile(vm),
                                Address1 = vm.Address1.Trim(),
                                Address2 = vm.Address2.Trim(),
                                CreatedBy = Convert.ToInt32(userId),
                                ModifiedBy = Convert.ToInt32(userId),
                                City = vm.City.Trim(),
                                StateId = vm.StateId,
                                CountryId = vm.CountryId,
                                CollegeCode=vm.CollegeCode,
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
            IGeneralResult<CollegeVM> result = new GeneralResult<CollegeVM>();
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
                    clg = (await _collegeRepo.GetAll()).Where(w => w.IsDeleted == false && w.IsActive == true && cid==0?true:w.ClientId==cid).FirstOrDefault();
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
                bool isExits = _collegeRepo.Any(x => x.CollegeName == vm.CollegeName && x.IsDeleted == false);
                if (isExits)
                {
                    result.Message = " CollegeName is already exists";
                }
                bool isexist = _collegeRepo.Any(x => x.CollegeCode == vm.CollegeCode && x.IsDeleted==false);
                if (isexist)
                {
                    result.Message = "CollegeCode is alredy exist";
                }
                else
                {
                    if (ModelState.IsValid && vm.CollegeId > 0 && clg.IsDeleted == false)
                    {
                        clg.CollegeName = vm.CollegeName.Trim();
                        clg.Logo = ProcessUploadFile(vm);
                        clg.Address1 = vm.Address1.Trim();
                        clg.Address2 = vm.Address2.Trim();
                        clg.CreatedBy = Convert.ToInt32(userId);
                        clg.ModifiedBy = Convert.ToInt32(userId);
                        clg.City = vm.City.Trim();
                        clg.StateId = vm.StateId;
                        clg.CountryId = vm.CountryId;
                        clg.CollegeCode = vm.CollegeCode.Trim();
                        clg.ContectPerson = vm.ContectPerson.Trim();
                        clg.ContectEmail = vm.ContectEmail.Trim();
                        clg.ContectPhone = vm.ContectPhone.Trim();

                        try
                        {
                            result.Data = (CollegeVM)await _collegeRepo.Update(clg);
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
        public async Task<IActionResult> DeleteCollege(int Id)
        {
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            IGeneralResult<CollegeVM> result = new GeneralResult<CollegeVM>();
            var res = await _collegeRepo.GetById(Id);
            if (res.IsDeleted == false)
            {
                res.IsActive = false;
                res.IsDeleted = true;
                await _collegeRepo.Update(res);
                result.IsSuccess = true;
                result.Message = "College Deleted Succesfully";
            }
            else
            {
                result.Message = "College does Not exist";
            }
            return Ok(result);
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

                res.IsActive = false;
                res.IsDeleted = true;
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
            return uniqueFileName;
        }
    }
    }

