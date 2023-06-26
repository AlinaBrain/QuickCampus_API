using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Collections;
using static Azure.Core.HttpHeader;

namespace QuickCampusAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampusController : ControllerBase
    {
        private readonly ICampusRepo _campusrepo;
        //private readonly ICountry _country;
        private readonly IStateRepo _staterepo;

        public CampusController(IConfiguration configuration, ICampusRepo campusrepo)
        {
            _campusrepo = campusrepo;
            //_country = country;
            //_staterepo = stateRepo;
        }

        [HttpGet]
        [Route("ManageCampus")]
        public async Task<IActionResult> ManageCampus(int WalkInId)
        {
            IGeneralResult<List<CampusGridViewModel>> result = new GeneralResult<List<CampusGridViewModel>>();

            var rec = await _campusrepo.GetAllCampus();
            var CampusList = rec.Where(x => x.WalkInID != null).ToList();
            var res = CampusList.Select(x => ((CampusGridViewModel)x)).ToList();

            if (res != null)
            {
                result.IsSuccess = true;
                result.Message = "List of Campus.";
                result.Data = res;
            }
            else
            {
                result.Message = "Campus not found!";
            }

            return Ok(result);
        }

        //[HttpGet]
        //[Route("Edit")]

        //public async Task<IActionResult> Edit(int id)
        //{
        //    var campus = await _campusrepo.GetCampusByID(id);
        //    var model = new CampusGridViewModel()
        //    {
        //        WalkInID = campus.WalkinID,
        //        JobDescription = campus.JobDescription,
        //        WalkInDate = campus.WalkInDate,
        //        Address1 = campus.Address1,
        //        Address2 = campus.Address2,
        //        City = campus.City,
        //        StateID = campus.StateID,
        //        CountryID = campus.CountryID,
        //        Colleges = campus.Colleges,
        //        Title = campus.Title
        //    };

        //    var StateList = Common.GetStateByCountryID(campus.CountryID ?? 0);
        //    model.States = StateList.Successful ? (StateList.Value as IEnumerable).OfType<StateModel>().Select(x => new SelectListItem() { Text = x.StateName, Value = x.StateID.ToString() }) : new List<SelectListItem>();
        //    model.Countries = (Common.GetAllCountries().Value as IEnumerable).OfType<CountryModel>().Select(x => new SelectListItem() { Text = x.CountryName, Value = x.CountryID.ToString() });
        //    return Ok(model);

        //}
        //[HttpGet]
        //[Route("Edit")]
        //public async Task<ActionResult> Edit(int Id)
        //{
        //    var campus = _campusrepo.GetCampusByID(Id);
        //    CampusGridViewModel model = new CampusGridViewModel()
        //    {
        //        WalkInID = campus.Id,
        //        JobDescription = campus.JobDescription,
        //        WalkInDate = campus.WalkInDate,
        //        Address1 = campus.Address1,
        //        Address2 = campus.Address2,
        //        City = campus.City,
        //        StateID = campus.StateID,
        //        CountryID = campus.CountryID,
        //        Colleges = campus.Colleges,
        //        Title = campus.Title
        //    };
        //    return Ok(model);
        //}

        [HttpGet]
        [Route("AddCampus")]
        public ActionResult AddCampus()
        {
            var model = new CampusGridViewModel()
            {
                States = new List<SelectListItem>() { },
                Countries = new List<SelectListItem>() { },
                Colleges = new List<CampusWalkInModel>()
            };
            return Ok(model);
        }


        //    [HttpPost]
        //    [Route("AddCampus")]
        //    public async Task<IActionResult> AddCampus(CampusGridViewModel campusGridViewModel)
        //    {
        //        IGeneralResult<CampusGridViewModel> result = new GeneralResult<CampusGridViewModel>();
        //        try
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                var FindCountry = _country.GetAll().Result.Where(x => x.CountryId == campusGridViewModel.CountryID).ToList();
        //                var FindStates = _staterepo.GetAll().Result.Where(x => x.StateId == campusGridViewModel.StateID);
        //                if (FindCountry == null)
        //                {
        //                    result.Message = "This Country is not listed for Country Table!";
        //                    return Ok(result);
        //                }
        //                else if (FindStates == null)
        //                {
        //                    result.Message = "State not found!";
        //                    return Ok(result);
        //                }
        //                else
        //                {
        //                    var campusDetails = _campusrepo.GetAll().Result.Where(x => x.CountryId == campusGridViewModel.CountryID && x.StateId == campusGridViewModel.StateID).FirstOrDefault();
        //                    if (campusDetails == null)
        //                    {
        //                        var ad = new CampusGridViewModel()
        //                        {
        //                            Title = campusGridViewModel.Title,
        //                            StateID = campusGridViewModel.StateID,
        //                            CountryID = campusGridViewModel.CountryID,
        //                            City = campusGridViewModel.City,
        //                            JobDescription = campusGridViewModel.JobDescription,
        //                            WalkInDate = campusGridViewModel.WalkInDate,
        //                            Address1 = campusGridViewModel.Address1,
        //                            Address2 = campusGridViewModel.Address2,



        //                        };

        //                        var campus = await _campusrepo.Add(campusDetails);
        //                        if (campus != null)
        //                        {
        //                            result.IsSuccess = true;
        //                            result.Message = "College Added Successfully.";
        //                            result.Data = campusGridViewModel;
        //                        }
        //                        else
        //                        {
        //                            result.Message = " Something Went wrong!";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        result.Message = "College Already Exist!";
        //                    }
        //                }
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            result.Message = "Server Error!";
        //        }
        //        return Ok(result);
        //    }

        //}

    }
}










