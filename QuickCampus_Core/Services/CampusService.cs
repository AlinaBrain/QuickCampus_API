using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Linq.Expressions;
using System.Security.Cryptography;
using static QuickCampus_Core.Common.common;

namespace QuickCampus_Core.Services
{
    public class CampusService : BaseRepository<BtprojecQuickcampustestContext, TblWalkIn>, ICampusRepo
    {
        private readonly BtprojecQuickcampustestContext _context;
        public CampusService(BtprojecQuickcampustestContext context, ICollegeRepo collegeRepo)
        {
            _context = context;

        }
        public async Task<IGeneralResult<CampusGridRequestVM>> AddOrUpdateCampus(CampusGridRequestVM vm)
        {
            IGeneralResult<CampusGridRequestVM> result = new GeneralResult<CampusGridRequestVM>();

            var isCountryExist = _context.MstCityStateCountries.Where(w => w.IsDeleted == false).Any(a => a.CountryId == vm.CountryId);
            var allCollages = _context.TblColleges.Where(s => s.IsDeleted == false).Select(s => s.CollegeId).ToList();
            var allStates = _context.MstCityStates.Where(w => w.IsDeleted == false).Select(s => s.StateId).ToList();
            var isStateExist = allStates.Any(a => a == vm.StateId);
            var allCity = await _context.MstCities.Where(m => m.IsDeleted == false).Select(c => c.CityId).ToListAsync();
            var isCityExist = allCity.Any(x => x == vm.City);

            foreach (var clg in vm.Colleges)
            {
                var checkclg = allCollages.Any(s => s == clg.CollegeId);
                if (!checkclg)
                {

                    result.Message = "TblCollege id " + clg.CollegeId + " does not exist";
                    return result;
                }

            }
            if (!isCountryExist)
            {

                result.Message = "Country is not exist";
                return result;
            }
            else if (!isStateExist)
            {

                result.Message = "State is not exist";
                return result;
            }
            else if (!isCityExist)
            {
                result.Message = "City is Not exist ";
                return result;
            }
            try
            {
                if (vm.WalkInID > 0)
                {
                    var campus = _context.TblWalkIns.Include(x => x.TblWalkInColleges).Where(x => x.WalkInId == vm.WalkInID).FirstOrDefault();
                    if (campus != null)
                    {
                        campus.WalkInDate = vm.WalkInDate;
                        campus.JobDescription = vm.JobDescription;
                        campus.Address1 = vm.Address1;
                        campus.Address2 = vm.Address2;
                        campus.City = vm.City;
                        campus.StateId = vm.StateId;
                        campus.CountryId = vm.CountryId;
                        campus.Title = vm.Title;
                        campus.CreatedDate = DateTime.Now;
                        campus.PassingYear = vm.PassingYear;
                        var walkindata = _context.Update(campus);
                        vm.WalkInID = walkindata.Entity.WalkInId;
                        if (campus.TblWalkInColleges != null)
                        {
                            _context.TblWalkInColleges.RemoveRange(campus.TblWalkInColleges);
                        }
                        foreach (var rec in vm.Colleges)
                        {
                            if (rec.IsIncludeInWalkIn)
                            {
                                TblWalkInCollege campusWalkInCollege = new TblWalkInCollege()
                                {
                                    WalkInId = campus.WalkInId,
                                    CollegeId = rec.CollegeId,
                                    ExamStartTime = TimeSpan.Parse(rec.ExamStartTime),
                                    ExamEndTime = TimeSpan.Parse(rec.ExamEndTime),
                                    StartDateTime = rec.StartDateTime,
                                };
                                var updatecampus = _context.TblWalkInColleges.Add(campusWalkInCollege);
                                rec.CampusId = updatecampus.Entity.CampusId;
                            }
                        }
                    }
                    var UpdateResult = _context.SaveChanges();

                    if (UpdateResult > 0)
                    {

                        result.IsSuccess = true;
                        result.Message = "Record Update Successfully";
                        result.Data = vm;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Something went wrong.";
                    }
                    return result;
                }

                var sv = new TblWalkIn()
                {
                    WalkInDate = vm.WalkInDate,
                    JobDescription = vm.JobDescription,
                    Address1 = vm.Address1,
                    Address2 = vm.Address2,
                    City = vm.City,
                    StateId = vm.StateId,
                    CountryId = vm.CountryId,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    Title = vm.Title,
                    PassingYear=vm.PassingYear

                };
                var walkin = _context.TblWalkIns.Add(sv);
                _context.SaveChanges();
                vm.WalkInID = walkin.Entity.WalkInId;

                foreach (var rec in vm.Colleges)
                {

                    TblWalkInCollege campusWalkInCollege = new TblWalkInCollege()
                    {

                        WalkInId = sv.WalkInId,
                        CollegeId = rec.CollegeId,
                        ExamStartTime = TimeSpan.Parse(rec.ExamStartTime),
                        ExamEndTime = TimeSpan.Parse(rec.ExamEndTime),
                        CampusId = rec.CampusId,
                        StartDateTime = rec.StartDateTime,

                    };
                    var collegeWalkin = _context.TblWalkInColleges.Add(campusWalkInCollege);
                    _context.SaveChanges();

                    rec.CampusId = collegeWalkin.Entity.CampusId;
                }
                result.IsSuccess = true;
                result.Message = "Record Saved Successfully";
                result.Data = vm;

            }

            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "Something went wrong";
            }
            return result;
        }


        private TblWalkInCollege getWalkin(int id)
        {
            TblWalkInCollege res = new TblWalkInCollege();
            res = _context.TblWalkInColleges.Include(i => i.College).Where(w => w.CampusId == id).FirstOrDefault();
            return res;
        }
        public async Task<IGeneralResult<List<CampusGridViewModel>>> GetAllCampus()
        {
            IGeneralResult<List<CampusGridViewModel>> result = new GeneralResult<List<CampusGridViewModel>>();

            var campusdata = _context.TblWalkIns.Where(x => x.IsDeleted == false)
            .Include(x => x.TblWalkInColleges).Include(x => x.State).Include(x => x.Country).OrderByDescending(x => x.WalkInDate).Select(x => new CampusGridViewModel()
            {
                WalkInID = x.WalkInId,
                Address1 = x.Address1,
                Address2 = x.Address2,
                City = x.City,
                StateId = x.StateId,
                CountryId = x.CountryId,
                JobDescription = x.JobDescription,
                WalkInDate = x.WalkInDate,
                IsActive = x.IsActive ?? false,
                Title = x.Title,
                Colleges = x.TblWalkInColleges.Select(y => new CampusWalkInModel()
                {

                    CampusId = y.CampusId,
                    CollegeId = y.CollegeId ?? 0,
                    ExamEndTime = y.ExamEndTime.Value.ToString(),
                    ExamStartTime = y.ExamStartTime.Value.ToString(),
                    IsIncludeInWalkIn = true,
                    StartDateTime = y.StartDateTime.Value,
                    CollegeName = _context.TblColleges.Where(z => z.CollegeId == y.CollegeId).First().CollegeName,
                    CollegeCode = _context.TblColleges.Where(z => z.CollegeId == y.CollegeId).First().CollegeCode,
                }).ToList(),
            }).ToList();

            if (campusdata.Any())
            {
                result.Data = campusdata;
                return result;
            }
            else
            {
                result.Message = "Something Went Wrong ";
            }
            return result;
        }

        public async Task<IGeneralResult<CampusGridViewModel>> GetCampusByID(int id)
        {
            IGeneralResult<CampusGridViewModel> result = new GeneralResult<CampusGridViewModel>();

            var campusData = _context.TblWalkIns.Where(x => x.IsDeleted == false && x.WalkInId == id).Include(x => x.TblWalkInColleges).Include(x => x.State).Include(x => x.Country).OrderByDescending(x => x.WalkInDate).Select(x => new CampusGridViewModel()
            {
                WalkInID = x.WalkInId,
                Address1 = x.Address1,
                Address2 = x.Address2,
                City = x.City,
                StateId = x.StateId,
                CountryId = x.CountryId,
                JobDescription = x.JobDescription,
                WalkInDate = x.WalkInDate.Value,
                IsActive = x.IsActive ?? false,
                Title = x.Title,
                Colleges = x.TblWalkInColleges.Where(z => z.WalkInId == x.WalkInId).Select(y => new CampusWalkInModel()
                {
                    CampusId = y.CampusId,
                    CollegeId = y.CollegeId ?? 0,
                    ExamEndTime = y.ExamEndTime.Value.ToString(),
                    ExamStartTime = y.ExamStartTime.Value.ToString(),
                    IsIncludeInWalkIn = true,
                    StartDateTime = y.StartDateTime.Value,
                    CollegeName = _context.TblColleges.Where(z=>z.CollegeId == y.CollegeId).First().CollegeName,
                    CollegeCode = _context.TblColleges.Where(z => z.CollegeId == y.CollegeId).First().CollegeCode,
                }).ToList(),
            }).FirstOrDefault();
            if (campusData != null)
            {
                CampusGridViewModel vmm = new CampusGridViewModel
                {
                    WalkInID = campusData.WalkInID,
                    Address1 = campusData.Address1,
                    Address2 = campusData.Address2,
                    City = campusData.City,
                    StateId = campusData.StateId,
                    CountryId = campusData.CountryId,
                    IsActive = campusData.IsActive,
                    Colleges = campusData?.Colleges,
                    JobDescription = campusData.JobDescription,
                    Title = campusData?.Title,
                    WalkInDate = campusData.WalkInDate
                };
                result.Data = vmm;
                result.IsSuccess = true;
                result.Message = "Campus fetched Successfully";
            }
            else
            {
                result.Message = "Data Not found";

            }
            return result;
        }
        private CampusWalkInModel GetCollegeDetails(int collegeid)
        {
            CampusWalkInModel statevm = new CampusWalkInModel();

            var cllgdetails = _context.TblColleges.Find(collegeid);
            statevm.CollegeId = cllgdetails.CollegeId;

            return statevm;
        }
        public async Task<IGeneralResult<CampusGridViewModel>> ActiveInActive(int id, bool status)
        {
            IGeneralResult<CampusGridViewModel> result = new GeneralResult<CampusGridViewModel>();
            result.Data = new CampusGridViewModel();
            TblWalkIn campus = new TblWalkIn();
            campus = _context.TblWalkIns.Where(x => x.WalkInId == id && x.IsDeleted == false).Include(x => x.State).Include(x => x.Country).Include(x => x.TblWalkInColleges).FirstOrDefault();

            if (campus == null)
            {
                result.IsSuccess = false;
                result.Message = "Record Not Found";
                return result;
            }
            campus.IsActive = status;

            _context.TblWalkIns.Update(campus);
            int st = _context.SaveChanges();

            if (st > 0)
            {
                result.Message = "Campus Upadte Successfully";
                result.IsSuccess = true;
            }
            else
            {
                result.Message = "Something went wrong";
                result.IsSuccess = false;
            }
            return result;
        }


        public async Task<IGeneralResult<string>> UpdateCampus(CampusGridRequestVM vm, int clientId, int userId)
        {
            IGeneralResult<string> result = new GeneralResult<string>();

            var isCountryExist = _context.MstCityStateCountries.Where(w => w.IsDeleted == false).Any(a => a.CountryId == vm.CountryId);
            var allCollages = _context.TblColleges.Where(s => s.IsDeleted == false).Select(s => s.CollegeId).ToList();
            var allStates = _context.MstCityStates.Where(w => w.IsDeleted == false).Select(s => s.StateId).ToList();
            var isStateExist = allStates.Any(a => a == vm.StateId);
            bool isExits = _context.TblWalkIns.Any(x => x.IsDeleted == false);


            foreach (var clg in vm.Colleges)
            {
                var checkclg = allCollages.Any(s => s == clg.CollegeId);
                if (!checkclg)
                {
                    result.IsSuccess = false;
                    result.Message = "TblCollege id " + clg.CollegeId + " is not exist";
                    return result;
                }

            }

            if (!isCountryExist)
            {
                result.IsSuccess = false;
                result.Message = "MstCity_State_Country is not exist";
                return result;
            }
            else if (!isStateExist)
            {
                result.IsSuccess = false;
                result.Message = "MstCity_State is not exist";
                return result;
            }

            try
            {
                if (vm.WalkInID > 0)
                {
                    TblWalkIn campus = _context.TblWalkIns.Where(x => x.WalkInId == vm.WalkInID && (clientId == 0 ? true : x.ClientId == clientId)).Include(x => x.State).Include(x => x.Country).Include(x => x.TblWalkInColleges).FirstOrDefault();
                    if (campus != null)
                    {
                        campus.WalkInId = vm.WalkInID;
                        campus.WalkInDate = vm.WalkInDate;
                        campus.JobDescription = vm.JobDescription;
                        campus.Address1 = vm.Address1;
                        campus.Address2 = vm.Address2;
                        campus.City = vm.City;
                        campus.StateId = vm.StateId;
                        campus.CountryId = vm.CountryId;
                        campus.Title = vm.Title;
                        campus.CreatedDate = DateTime.Now;
                        _context.Update(campus);

                        if (campus.TblWalkInColleges != null)
                        {
                            _context.TblWalkInColleges.RemoveRange(campus.TblWalkInColleges);
                        }

                        foreach (var rec in vm.Colleges)
                        {
                            if (rec.IsIncludeInWalkIn)
                            {
                                TblWalkInCollege campusWalkInCollege = new TblWalkInCollege()
                                {
                                    WalkInId = campus.WalkInId,
                                    CollegeId = rec.CollegeId,
                                    ExamStartTime = TimeSpan.Parse(rec.ExamStartTime),
                                    ExamEndTime = TimeSpan.Parse(rec.ExamEndTime),
                                    StartDateTime = rec.StartDateTime,

                                    IsCompleted = null
                                };
                                _context.TblWalkInColleges.Update(campusWalkInCollege);
                            }
                        }
                    }
                    var UpdateResult = _context.SaveChanges();
                    if (UpdateResult > 0)
                    {
                        result.IsSuccess = true;
                        result.Message = "Record Update Successfully";
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Something went wrong.";
                    }
                    return result;
                }

                var sv = new TblWalkIn()
                {
                    WalkInId = vm.WalkInID,
                    WalkInDate = vm.WalkInDate,
                    JobDescription = vm.JobDescription,
                    Address1 = vm.Address1,
                    Address2 = vm.Address2,
                    City = vm.City,
                    StateId = vm.StateId,
                    CountryId = vm.CountryId,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    Title = vm.Title,
                    ClientId = clientId == 0 ? 0 : clientId
                };
                _context.TblWalkIns.Update(sv);
                _context.SaveChanges();

                foreach (var rec in vm.Colleges)
                {
                    if (rec.IsIncludeInWalkIn)
                    {
                        TblWalkInCollege campusWalkInCollege = new TblWalkInCollege()
                        {
                            WalkInId = sv.WalkInId,
                            CollegeId = rec.CollegeId,
                            ExamStartTime = TimeSpan.Parse(rec.ExamStartTime),
                            ExamEndTime = TimeSpan.Parse(rec.ExamEndTime),
                            StartDateTime = rec.StartDateTime,
                            IsCompleted = null
                        };
                        _context.TblWalkInColleges.Update(campusWalkInCollege);
                    }
                }

                int response = _context.SaveChanges();
                if (response > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Record Updated Successfully";

                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Something went wrong.";
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "Something went wrong";
            }

            return result;
        }

        public async Task<IGeneralResult<CampusGridViewModel>> DeleteCampus(int id)
        {
            IGeneralResult<CampusGridViewModel> result = new GeneralResult<CampusGridViewModel>();
            result.Data = new CampusGridViewModel();
            TblWalkIn campus = new TblWalkIn();
            campus = _context.TblWalkIns.Where(x => x.WalkInId == id  && x.IsDeleted == false).Include(x => x.State).Include(x => x.Country).Include(x => x.TblWalkInColleges).FirstOrDefault();
            if (campus == null)
            {
                result.IsSuccess = false;
                result.Message = "Record Not Found";
                return result;
            }
            campus.IsDeleted = true;
            campus.IsActive = false;
            _context.TblWalkIns.Update(campus);
            int st = _context.SaveChanges();
            if (st > 0)
            {
                result.Message = "Campus delete Successfully";
                result.IsSuccess = true;
            }
            else
            {
                result.Message = "Something went wrong";
                result.IsSuccess = false;
            }
            return result;
        }
    }
}