using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using QuickCampus_Core.Common;
using QuickCampus_Core.Common.Helper;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using static QuickCampus_Core.Common.common;
namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICountryRepo _countryRepo;
        private readonly IStateRepo _stateRepo;
        private readonly ICityRepo _cityRepo;
        private readonly IConfiguration _config;
        private readonly ProcessUploadFile _uploadFile;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private string baseUrl;
        private readonly IMstSkillsRepo _mstSkillsRepo;
        private IUserRepo _userRepo;
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private string _jwtSecretKey;

        public CommonController(ICountryRepo countryRepo, IStateRepo stateRepo,
            ICityRepo cityRepo, IConfiguration configuration, ProcessUploadFile uploadFile, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, 
            IMstSkillsRepo mstSkillsRepo, IUserRepo userRepo, IUserAppRoleRepo userAppRoleRepo
            )
        {
            _countryRepo = countryRepo;
            _stateRepo = stateRepo;
            _cityRepo = cityRepo;
            _config = configuration;
            _uploadFile = uploadFile;
            _hostingEnvironment = hostingEnvironment;
            baseUrl = _config.GetSection("APISitePath").Value ?? "";
            _mstSkillsRepo=mstSkillsRepo;
            _userRepo=userRepo;
            _userAppRoleRepo=userAppRoleRepo;
            baseUrl = _config.GetSection("APISitePath").Value;
            _jwtSecretKey = _config["Jwt:Key"] ?? "";
        }

        [HttpGet]
        [Route("GetCountries")]
        public async Task<IActionResult> GetAllCountry()
        {
            IGeneralResult<List<CountryVM>> result = new GeneralResult<List<CountryVM>>();
            try
            {
                var countryList = (await _countryRepo.GetAll()).Where(x => x.IsDeleted == false && x.IsActive == true).ToList().OrderBy(x => x.CountryName);
                var res = countryList.Select(x => ((CountryVM)x)).ToList();
                if (res != null && res.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Country fetched successfully.";
                    result.Data = res;
                    result.TotalRecordCount = res.Count();
                }
                else
                {
                    result.Message = "No record found";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetStatesByCountry")]
        public async Task<IActionResult> GetAllState(int countryId)
        {
            IGeneralResult<List<StateVM>> result = new GeneralResult<List<StateVM>>();

            List<MstCityState> stateList = new List<MstCityState>();
            try
            {
                stateList = (await _stateRepo.GetAll()).Where(x => x.IsDeleted == false && x.CountryId == countryId).ToList();
                var response = stateList.Select(x => (StateVM)x).ToList();
                if (stateList.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "States fetched successfully";
                    result.Data = response.OrderBy(x=>x.StateName).ToList();
                    result.TotalRecordCount = response.Count();
                }
                else
                {
                    result.Message = "No data found!";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllCity")]
        public IActionResult GetAllCityByState(int stateId)
        {
            IGeneralResult<dynamic> result = new GeneralResult<dynamic>();
            List<MstCity> CityList = new List<MstCity>();
            var cityTotalCount = 0;
            try
            {
                CityList = _cityRepo.GetAllQuerable().Where(x => x.IsActive == true && x.IsDeleted == false && x.StateId == stateId).ToList();
                cityTotalCount = CityList.Count;

                var response = CityList.Select(x => new
                { 
                    x.CityId,
                    x.CityName
                }).ToList();
                if (cityTotalCount > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Cities fetched successfully.";
                    result.Data = response.OrderBy(x=>x.CityName).ToList();
                    result.TotalRecordCount = cityTotalCount;
                }
                else
                {
                    result.Message = "No city found!";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error " + ex.Message;
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("UploadFiles")]
        public IActionResult ProcessUploadFile(List<IFormFile> Files)
        {
            IGeneralResult<List<string>> result = new GeneralResult<List<string>>();
            try
            {
                List<string> url = new List<string>();
                if (Files.Count > 0)
                {
                  
                    int UploadFileCount = 0;
                    foreach (IFormFile file in Files)
                    {
                        var res =_uploadFile.GetUploadFile(file);
                        if (res.IsSuccess)
                        {
                            UploadFileCount += 1;
                            url.Add(Path.Combine(baseUrl, res.Data));
                        }
                    }
                    result.IsSuccess = true;
                    result.Message = "Files upload successfully.";
                    result.Data = url;
                    result.TotalRecordCount = UploadFileCount;
                    return Ok(result);

                }
                else
                {
                    result.Message = "Please add atleast one File to upload.";
                }
            }
            catch(Exception ex)
            {
                result.Message = "Server error " + ex.Message;
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("AddSkills")]
        public async Task<IActionResult> AddSkills(MstSkillsVm Vm)
        {
            IGeneralResult<MstSkillsVm> result = new GeneralResult<MstSkillsVm>();
            try
            {
                if (Vm == null)
                {
                    result.Message = "Your Model request in Invalid";
                    return Ok(result);
                }
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await _userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                }
                if (ModelState.IsValid)
                {
                    MstSkillsVm mstskills = new MstSkillsVm
                    {
                        SkillName=Vm.SkillName,
                        ClientId= Convert.ToInt32(LoggedInUserClientId)
                    };
                    var mstskillsdata =await _mstSkillsRepo.Add(mstskills.ToMstSkillDbMOdel());
                    if(mstskillsdata.SkillId >0)
                    {
                        result.IsSuccess = true;
                        result.Message = "Skills added successfully";
                        result.Data = (MstSkillsVm)mstskillsdata;
                    }
                    else
                    {
                        result.Message = "Something went wrong.";
                    }
                }
                else
                {
                    result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
                }

            }
            catch(Exception ex)
            {
                result.Message = "Server Error" + ex.Message;
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllSkill")]
        public async Task<IActionResult> GetAllSkill(int ClientId,string? search, DataTypeFilter Datatype, int pageStart = 1, int pageSize = 10)
        {
            IGeneralResult<List<GetMstSkillVm>> result = new GeneralResult<List<GetMstSkillVm>>();
            try
            {
                var _jwtSecretKey = _config["Jwt:Key"];
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserRole != null && LoggedInUserRole.RoleId == (int)AppRole.Admin)
                {
                    var newPageStart = 0;
                    if (pageStart > 0)
                    {
                        var startPage = 1;
                        newPageStart = (pageStart - startPage) * pageSize;

                        List<MstSkill> Mstdata = new List<MstSkill>();

                        Mstdata = await _mstSkillsRepo.GetAll(x => x.IsDelete == false && (Datatype == DataTypeFilter.OnlyInActive ? x.IsActive == true : (Datatype == DataTypeFilter.OnlyInActive ? x.IsActive == false : true)));

                        //var roleData = _roleRepo.GetAll(x => x.IsActive == true && x.IsDeleted == false).Result.FirstOrDefault();
                        //var userAppRole = _userAppRoleRepo.GetAll(x => x.UserId == x.UserId).Result.FirstOrDefault();
                        //var userAppRoleId = userAppRole != null ? userAppRole.RoleId : 0;
                        List<GetMstSkillVm> data = new List<GetMstSkillVm>();
                        data.AddRange(Mstdata.Select(x => new GetMstSkillVm
                        {
                            SkillId = x.SkillId,
                            SkillName = x.SkillName,
                            IsActive = x.IsActive,
                            ClientId = x.ClientId,
                        }).ToList().Where(x=>(x.SkillName.Contains(search ?? ""))).OrderByDescending(x => x.SkillId).ToList());
                        result.Data = data.Skip(newPageStart).Take(pageSize).ToList();
                        result.IsSuccess = true;
                        result.Message = "Client fetched Successfully";
                        result.TotalRecordCount = data.Count;
                        return Ok(result);
                       
                    }
                    else
                    {
                        result.Message = "Access Denied";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server Error " + ex.Message;
            }
            return Ok(result);
        }

    }
}
