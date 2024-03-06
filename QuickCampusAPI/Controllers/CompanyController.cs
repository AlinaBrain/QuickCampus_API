﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepo _companyRepo;

        public CompanyController(ICompanyRepo companyRepo)
        {
            _companyRepo=companyRepo;
        }
        //[Authorize(Roles = "GetAllCountry")]
        [HttpGet]
        [Route("GetAllCompany")]
        public async Task<IActionResult> GetAllCompany()
        {
            IGeneralResult<List<CompanyVm>> result = new GeneralResult<List<CompanyVm>>();
           
           
            try
            {
                var companylist = (await _companyRepo.GetAll()).Where(x => x.Isdeleted != true) .ToList().OrderByDescending(x => x.CompanyId);

                // var clientList = (await _countryrepo.GetAll()).Where(x => x.IsDeleted != true && x.CountryName.Contains(countryName)).ToList();


                var res = companylist.Select(x => ((CompanyVm)x)).ToList();
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
        [Route("GetCompanyById")]
        public async Task<IActionResult> GetCompanyById(int Companyid)
        {
            IGeneralResult<CompanyVm> result = new GeneralResult<CompanyVm>();
            var res = await _companyRepo.GetById(Companyid);
            if (res.Isdeleted == false && res.IsActive == true)
            {
                result.Data = (CompanyVm)res;
                result.IsSuccess = true;
                result.Message = "Company details getting succesfully";
            }
            else
            {
                result.Message = "Company does Not exist";
            }
            return Ok(result);
        }
        //[Authorize(Roles = "AddCountry")]
        [HttpPost]
        [Route("AddCompany")]
        public async Task<IActionResult> AddCompany(CompanyVm companyVM)
        {
            IGeneralResult<CompanyVm> result = new GeneralResult<CompanyVm>();
           
            if (companyVM != null)
            {
                if (ModelState.IsValid)
                {
                    bool isExits = _companyRepo.Any(x => x.CompanyName == companyVM.CompanyName && x.Isdeleted == false);
                    if (isExits)
                    {
                        result.Message = " Company Name is already exists";
                    }
                    else
                    {
                        {
                            int recordcount = (await _companyRepo.GetAll()).Count();
                            CompanyVm companyVm = new CompanyVm
                            {
                                CompanyName = companyVM.CompanyName.Trim(),
                                IsActive = true,
                                Isdeleted = false,
                                CompanyId = recordcount + 1
                            };
                            try
                            {
                                var companydata = await _companyRepo.Add(companyVm.ToDbModel());
                                result.Data = (CompanyVm)companydata;
                                
                                result.Message = "Company added successfully";
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
        [Route("EditCompany")]
        public async Task<IActionResult> EditCompany(CompanyVm vm)
        {
            IGeneralResult<CompanyVm> result = new GeneralResult<CompanyVm>();
            
            if (_companyRepo.Any(x => x.CompanyName == vm.CompanyName && x.IsActive == true && x.CompanyId != vm.CompanyId))
            {
                result.Message = "Email Already Registered!";
            }
            else
            {
                var res = await _companyRepo.GetById(vm.CompanyId);
                bool isDeleted = (bool)res.Isdeleted ? true : false;
                if (isDeleted)
                {
                    result.Message = " Company does Not Exist";
                    return Ok(result);
                }

                if (ModelState.IsValid && vm.CompanyId > 0 && res.Isdeleted == false)
                {
                    res.CompanyName = vm.CompanyName.Trim();
                    res.IsActive = true;
                    res.CompanyId = vm.CompanyId;
                    res.Isdeleted = false;

                    try
                    {
                        result.Data = (CompanyVm)await _companyRepo.Update(res);
                        result.Message = "Company updated successfully";
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
        [Route("DeleteCompany")]
        public async Task<IActionResult> DeleteCompany(int companyid)
        {
            
            IGeneralResult<CompanyVm> result = new GeneralResult<CompanyVm>();
            var res = await _companyRepo.GetById(companyid);
            if (res.Isdeleted == false)
            {
                res.IsActive = false;
                res.Isdeleted = true;
                await _companyRepo.Update(res);
                result.IsSuccess = true;
                result.Message = "Company Deleted Succesfully";
            }
            else
            {
                result.Message = "Company does Not exist";
            }
            return Ok(result);
        }

    }
}
