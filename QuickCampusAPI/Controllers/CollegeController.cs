using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;

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
        public CollegeController(ICollegeRepo collegeRepo, IConfiguration config, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _collegeRepo = collegeRepo;
            _config = config;
            _hostingEnvironment = hostingEnvironment;   
        }

        [Authorize(Roles = "GetAllCollege")]
        [HttpGet]
        [Route("GetAllCollege")]
        public async Task<IActionResult> GetAllCollege()
        {
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            IGeneralResult<List<CollegeVM>> result = new GeneralResult<List<CollegeVM>>();
            try
            {
                var collegeList = (await _collegeRepo.GetAll()).Where(x => x.IsDeleted == false || x.IsDeleted == null).ToList();
                var res = collegeList.Select(x => ((CollegeVM)x)).ToList();
                if (res != null)
                {
                    result.IsSuccess = true;
                    result.Message = "College get successfully";
                    result.Data = res;
                }
                else
                {
                    result.Message = "College List Not Found";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }

        [Authorize(Roles = "CollegeDetails")]
        [HttpGet]
        [Route("CollegeDetails")]
        public async Task<IActionResult> CollegeDetails(int Id)
        {
            var _jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            IGeneralResult<CollegeVM> result = new GeneralResult<CollegeVM>();
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
        public async Task<IActionResult> AddCollege([FromBody] CollegeVM vm)
        {
            IGeneralResult<CollegeVM> result = new GeneralResult<CollegeVM>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (vm != null)
            {
                if (ModelState.IsValid)
                {

                    CollegeVM collegeVM = new CollegeVM
                    {
                        CollegeName = vm.CollegeName,
                        Logo = vm.Logo,
                        Address1 = vm.Address1,
                        Address2 = vm.Address2,
                        CreatedBy = Convert.ToInt32(userId),
                        ModifiedBy = Convert.ToInt32(userId),
                        City = vm.City,
                        StateId = vm.StateId,
                        CountryId = vm.CountryId,
                        CollegeCode = vm.CollegeCode,
                        ContectPerson = vm.ContectPerson,
                        ContectEmail = vm.ContectEmail,
                        ContectPhone = vm.ContectPhone,
                    };
                    try
                    {
                        var collegedata = await _collegeRepo.Add(collegeVM.ToCollegeDbModel());
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
        public async Task<IActionResult> EditCollege([FromBody] CollegeVM vm)
        {
            IGeneralResult<CollegeVM> result = new GeneralResult<CollegeVM>();
            var _jwtSecretKey = _config["Jwt:Key"];
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);

            if (vm != null)
            {
                var res = await _collegeRepo.GetById(vm.CollegeId);

                if (res == null)
                {
                    result.Message = " College does Not Exist";
                    return Ok(result);
                }
                bool isDeleted = (bool)res.IsDeleted ? true : false;
                if (isDeleted)
                {
                    result.Message = " College does Not Exist";
                    return Ok(result);
                }

                if (ModelState.IsValid && vm.CollegeId > 0 && res.IsDeleted == false)
                {
                    res.CollegeName = vm.CollegeName;
                    res.Logo = vm.Logo;
                    res.Address1 = vm.Address1;
                    res.Address2 = vm.Address2;
                    res.CreatedBy = Convert.ToInt32(userId);
                    res.ModifiedBy = Convert.ToInt32(userId);
                    res.City = vm.City;
                    res.StateId = vm.StateId;
                    res.CountryId = vm.CountryId;
                    res.CollegeCode = vm.CollegeCode;
                    res.ContectPerson = vm.ContectPerson;
                    res.ContectEmail = vm.ContectEmail;
                    res.ContectPhone = vm.ContectPhone;
                    res.ModifiedDate = DateTime.Now;

                    try
                    {
                        result.Data = (CollegeVM)await _collegeRepo.Update(res);
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
        //private string ProcessUploadFile(CollegeVM model)
        //{
        //    string uniqueFileName = null;
        //    if (model.file!= null)
        //    {
        //        string photoUoload = Path.Combine(_hostingEnvironment.WebRootPath, "Image");
        //        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.file.FileName;
        //        string filepath = Path.Combine(photoUoload, uniqueFileName);
        //        using (var filename = new FileStream(filepath, FileMode.Create))
        //        {
        //            model.file.CopyTo(filename);
        //        }
        //    }
        //    return uniqueFileName;
        //}

    }
}
