using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusRepo _statusrepo;

        public StatusController(IStatusRepo statusRepo)
        {
            _statusrepo=statusRepo;
        }
        [HttpGet]
        [Route("GetAllStatus")]
        public async Task<IActionResult> GetAllStatus()
        {
            IGeneralResult<List<StatusVm>> result = new GeneralResult<List<StatusVm>>();
            try
            {
                var statuslist = (await _statusrepo.GetAll()).Where(x => x.IsDeleted == false && x.IsActive==true).ToList().OrderByDescending(x => x.StatusId);

                // var clientList = (await _countryrepo.GetAll()).Where(x => x.IsDeleted != true && x.CountryName.Contains(countryName)).ToList();


                var res = statuslist.Select(x => ((StatusVm)x)).ToList();
                if (res != null && res.Count() > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Company List";
                    result.Data = res;
                    result.TotalRecordCount = res.Count();
                }
                else
                {
                    result.Message = "Company List Not Found";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Ok(result);
        }
        //[Authorize(Roles = "GetCountryById")]
        [HttpGet]
        [Route("GetStatusById")]
        public async Task<IActionResult> GetStatusById(int statusid)
        {
            IGeneralResult<StatusVm> result = new GeneralResult<StatusVm>();
            var res = await _statusrepo.GetById(statusid);
            if (res != null)
            {
                result.Data = (StatusVm)res;
                result.IsSuccess = true;
                result.Message = "Status details getting succesfully";
            }
            else
            {
                result.Message = "Status does Not exist";
            }
            return Ok(result);
        }
        //[Authorize(Roles = "AddCountry")]
        [HttpPost]
        [Route("AddCompany")]
        public async Task<IActionResult> AddStatus(StatusVm statusVm)
        {
            IGeneralResult<StatusVm> result = new GeneralResult<StatusVm>();

            if (statusVm != null)
            {
                if (ModelState.IsValid)
                {
                    bool isExits = _statusrepo.Any(x => x.StatusName == statusVm.StatusName && x.IsDeleted == false);
                    if (isExits)
                    {
                        result.Message = " Status Name is already exists";
                    }
                    else
                    {
                        {
                            int recordcount = (await _statusrepo.GetAll()).Count();
                            StatusVm statusvm = new StatusVm
                            {
                                StatusName = statusVm.StatusName.Trim(),
                                IsActive = true,
                                IsDeleted = false,
                                StatusId = recordcount + 1
                            };
                            try
                            {
                                var companydata = await _statusrepo.Add(statusVm.ToDbModel());
                                result.Data = (StatusVm)companydata;

                                result.Message = "Status added successfully";
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
        //[Authorize(Roles = "EditCountry")]
        [HttpPost]
        [Route("EditStatus")]
        public async Task<IActionResult> EditStatus(StatusVm vm)
        {
            IGeneralResult<StatusVm> result = new GeneralResult<StatusVm>();

            if (_statusrepo.Any(x => x.StatusName == vm.StatusName && x.IsActive == true && x.StatusId != vm.StatusId))
            {
                result.Message = "Status Name Already Registered!";
            }
            else
            {
                var res = await _statusrepo.GetById(vm.StatusId);
                bool isDeleted = (bool)res.IsDeleted ? true : false;
                if (isDeleted)
                {
                    result.Message = "Status does Not Exist";
                    return Ok(result);
                }

                if (ModelState.IsValid && vm.StatusId > 0 && res.IsDeleted == false)
                {
                    res.StatusName = vm.StatusName.Trim();
                    res.IsActive = true;
                    res.StatusId = vm.StatusId;
                    res.IsDeleted = false;

                    try
                    {
                        result.Data = (StatusVm)await _statusrepo.Update(res);
                        result.Message = "Status updated successfully";
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
        //[Authorize(Roles = "DeleteCountry")]
        [HttpDelete]
        [Route("DeleteStatus")]
        public async Task<IActionResult> DeleteStatus(int statusid)
        {

            IGeneralResult<StatusVm> result = new GeneralResult<StatusVm>();
            var res = await _statusrepo.GetById(statusid);
            if (res.IsDeleted == false)
            {
                res.IsActive = false;
                res.IsDeleted = true;
                await _statusrepo.Update(res);
                result.IsSuccess = true;
                result.Message = "Status Deleted Succesfully";
            }
            else
            {
                result.Message = "Status does Not exist";
            }
            return Ok(result);
        }
    }
}
