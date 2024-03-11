using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace QuickCampusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionRepo _sectionrepo;
        private readonly IConfiguration _config;

        public SectionController(ISectionRepo sectionRepo,IConfiguration configuration)
        {
           _sectionrepo= sectionRepo;
            _config= configuration;
        }
        [Authorize(Roles = "GetAllSection")]
        [HttpGet]
        [Route("GetAllSection")]
        public async Task<IActionResult> GetAllSection(int clientid)
        {
            IGeneralResult<List<SectionVm>> result = new GeneralResult<List<SectionVm>>();
            var _jwtSecretKey = _config["Jwt:Key"];

            int cid = 0;
            var jwtSecretKey = _config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var isSuperAdmin = JwtHelper.isSuperAdminfromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var newPageStart = 0;

            if (isSuperAdmin)
            {
                cid = clientid;
            }
            else
            {
                cid = string.IsNullOrEmpty(clientId) ? 0 : Convert.ToInt32(clientId);
            }
            List<Section> Sectionlist = new List<Section>();
            var cityTotalCount = 0;
            try
            {
                if (isSuperAdmin)
                {
                    cityTotalCount = (await _sectionrepo.GetAll()).Where(x => (cid == 0 ? true : x.ClentId == cid)).Count();
                    Sectionlist = (await _sectionrepo.GetAll()).Where(x => (cid == 0 ? true : x.ClentId == cid)).ToList();
                }
                else
                {
                    cityTotalCount = (await _sectionrepo.GetAll()).Where(x => (cid == 0 ? true : x.ClentId == cid)).Count();
                    Sectionlist = (await _sectionrepo.GetAll()).Where(x => (cid == 0 ? true : x.ClentId == cid)).ToList();
                }
                var response = Sectionlist.Select(x => (SectionVm)x).ToList();
                if (Sectionlist.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Section get successfully";
                    result.Data = response;
                    result.TotalRecordCount = Sectionlist.Count;
                }
                else
                {
                    result.IsSuccess = true;
                    result.Message = "Section list not found!";
                    result.Data = null;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }
        [Authorize(Roles = "AddSection")]
        [HttpPost]
        [Route("AddSection")]
        public async Task<IActionResult> AddSection(SectionVm vm, int clientid)
        {
            IGeneralResult<SectionVm> result = new GeneralResult<SectionVm>();
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
                    string pattern = @"^[a-zA-Z][a-zA-Z\s]*$";
                    string input = vm.Section1;
                    Match m = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                    if (!m.Success)
                    {
                        result.Message = "Only alphabetic characters are allowed in the name.";
                        return Ok(result);
                    }


                    {
                        SectionVm sectionVm = new SectionVm()
                        {
                            Section1 = vm.Section1,
                            SectionId = cid,
                            SortOrder = vm.SortOrder,
                            ClentId = cid,
                               
                            };
                            try
                            {
                                var sectiondata = await _sectionrepo.Add(sectionVm.ToSectionDbModel());
                                result.Data = (SectionVm)sectiondata;
                                result.Message = "City added successfully";
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
            return Ok(result);
            }
           
        }
    }

