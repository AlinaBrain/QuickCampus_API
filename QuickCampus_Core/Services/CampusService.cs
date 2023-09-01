using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace QuickCampus_Core.Services
{
    public class CampusService : BaseRepository<QuikCampusDevContext, WalkIn>, ICampusRepo
    {
        private readonly QuikCampusDevContext _context;
        public CampusService(QuikCampusDevContext context,ICollegeRepo collegeRepo)
        {
            _context = context;

        }

        public Task<IEnumerable<CampusGridViewModel>> Add(CampusGridViewModel campusGridViewModel)
        {
            throw new NotImplementedException();
        }


        public async Task<IGeneralResult<string>> AddCampus(CampusGridRequestVM vm, int clientId, int userId)
        {
            IGeneralResult<string> result = new GeneralResult<string>();

            var isCountryExist = _context.Countries.Where(w => w.IsDeleted == false).Any(a => a.CountryId == vm.CountryID);
            var allCollages = _context.Colleges.Where(s => s.IsDeleted == false).Select(s => s.CollegeId).ToList();
            var allStates = _context.States.Where(w => w.IsDeleted == false).Select(s => s.StateId).ToList();
            var isStateExist = allStates.Any(a => a == vm.StateID);
            bool isExits = _context.WalkIns.Any(x => x.Title == vm.Title && x.IsDeleted == false);
            if (isExits)
            {
                result.Message = " Title is already exists";
                return result;
            }

            foreach (var clg in vm.Colleges)
            {
                var checkclg = allCollages.Any(s => s == clg.CollegeId);
                if (!checkclg)
                {
                    result.IsSuccess = false;
                    result.Message = "College id " + clg.CollegeId + " is not exist";
                    return result;
                }
                var checkstate = allStates.Any(s => s == clg.StateId);
                if(!checkstate)
                {
                    result.IsSuccess = false;
                    result.Message = "state id " + clg.StateId + " is not exist";
                    return result;
                }
            }

            if (!isCountryExist)
            {
                result.IsSuccess = false;
                result.Message = "Country is not exist";
                return result;
            }
            else if (!isStateExist)
            {
                result.IsSuccess = false;
                result.Message = "State is not exist";
                return result;
            }

            try
            {
                if (vm.WalkInID > 0)
                {
                    WalkIn campus = _context.WalkIns.Where(x => x.WalkInId == vm.WalkInID && (clientId==0?true:x.ClientId == clientId)).Include(x => x.State).Include(x=>x.Country).Include(x => x.CampusWalkInColleges).FirstOrDefault();
                    if (campus != null)
                    {
                        campus.WalkInDate = vm.WalkInDate;
                        campus.JobDescription = vm.JobDescription;
                        campus.Address1 = vm.Address1;
                        campus.Address2 = vm.Address2;
                        campus.City = vm.City;
                        campus.StateId = vm.StateID;
                        campus.CountryId = vm.CountryID;
                        campus.Title = vm.Title;
                        campus.CreatedDate = DateTime.Now;
                        _context.Update(campus);

                        if (campus.CampusWalkInColleges != null)
                        {
                            _context.CampusWalkInColleges.RemoveRange(campus.CampusWalkInColleges);
                        }

                        foreach (var rec in vm.Colleges)
                        {
                            if (rec.IsIncludeInWalkIn)
                            {
                                CampusWalkInCollege campusWalkInCollege = new CampusWalkInCollege()
                                {
                                    WalkInId = campus.WalkInId,
                                    CollegeId = rec.CollegeId,
                                    ExamStartTime = TimeSpan.Parse(rec.ExamStartTime),
                                    ExamEndTime = TimeSpan.Parse(rec.ExamEndTime),
                                    CollegeCode = rec.CollegeCode,
                                    StartDateTime = rec.StartDateTime,
                                    IsCompleted = null
                                };
                                _context.CampusWalkInColleges.Add(campusWalkInCollege);
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

                var sv = new WalkIn()
                {
                    WalkInDate = vm.WalkInDate,
                    JobDescription = vm.JobDescription,
                    Address1 = vm.Address1,
                    Address2 = vm.Address2,
                    City = vm.City,
                    StateId = vm.StateID,
                    CountryId = vm.CountryID,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    Title = vm.Title,
                    ClientId = clientId == 0 ? 0 : clientId
                };
                _context.WalkIns.Add(sv);
                _context.SaveChanges();

                foreach (var rec in vm.Colleges)
                {
                    if (rec.IsIncludeInWalkIn)
                    {
                        CampusWalkInCollege campusWalkInCollege = new CampusWalkInCollege()
                        {
                            
                            WalkInId = sv.WalkInId,
                            CollegeId = rec.CollegeId,
                            ExamStartTime = TimeSpan.Parse(rec.ExamStartTime),
                            ExamEndTime = TimeSpan.Parse(rec.ExamEndTime),
                            CollegeCode = rec.CollegeCode,
                            StartDateTime = rec.StartDateTime,
                            IsCompleted = null
                        };
                        _context.CampusWalkInColleges.Add(campusWalkInCollege);
                    }
                }

                int response = _context.SaveChanges();
                if (response > 0)
                {
                    
                    result.IsSuccess = true;
                    result.Message = "Record Saved Successfully";
                    
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


        private CampusWalkInCollege getWalkin(int id)
        {
            CampusWalkInCollege res = new CampusWalkInCollege();
            res = _context.CampusWalkInColleges.Include(i => i.College).Where(w => w.CampusId == id).FirstOrDefault();
            return res;
        }
        public async Task<IEnumerable<CampusGridViewModel>> GetAllCampus(int clientId, bool isSuperAdmin,int pageStart,int pageSize)
        {

            if (isSuperAdmin)
            {


                var campuse = _context.WalkIns.Where(x => x.IsDeleted == false && (clientId == 0 ? true : x.ClientId == clientId)).Include(x => x.CampusWalkInColleges).Include(x => x.State).Include(x => x.Country).OrderByDescending(x => x.WalkInDate).Select(x => new CampusGridViewModel()
                {
                    WalkInID = x.WalkInId,
                    Address1 = x.Address1,
                    Address2 = x.Address2,
                    City = x.City,
                    StateID = x.StateId,
                    StateName = x.StateId > 0 ? x.State.StateName : "",
                    CountryID = x.CountryId,
                    CountryName = x.CountryId > 0 ? x.Country.CountryName : "",
                    CreatedDate = x.CreatedDate,
                    JobDescription = x.JobDescription,
                    WalkInDate = x.WalkInDate,
                    IsActive = x.IsActive ?? false,
                    Title = x.Title,
                    Colleges = x.CampusWalkInColleges.Select(y => new CampusWalkInModel()
                    {
                        CollegeCode = y.CollegeCode,
                        CollegeId = y.CollegeId ?? 0,
                        CollegeName = y.College.CollegeName,
                        ExamEndTime = y.ExamEndTime.Value.ToString(),
                        ExamStartTime = y.ExamStartTime.Value.ToString()

                    }).ToList(),
                }).Skip(pageStart).Take(pageSize).ToList();
                if (campuse.Any())
                {
                    return campuse.ToList();
                }
                else
                {
                    return new List<CampusGridViewModel>();
                }

            }

            else
            {
                var campuses = _context.WalkIns.Where(x => x.IsDeleted == false && x.ClientId == clientId).Include(x => x.CampusWalkInColleges).Include(x => x.State).Include(x => x.Country).OrderByDescending(x => x.WalkInDate).Select(x => new CampusGridViewModel()
                {
                    WalkInID = x.WalkInId,
                    Address1 = x.Address1.Trim(),
                    Address2 = x.Address2.Trim(),
                    City = x.City.Trim(),
                    StateID = x.StateId,
                    StateName = x.StateId > 0 ? x.State.StateName : "",
                    CountryID = x.CountryId,
                    CountryName = x.CountryId > 0 ? x.Country.CountryName : "",
                    CreatedDate = x.CreatedDate,
                    JobDescription = x.JobDescription.Trim(),
                    WalkInDate = x.WalkInDate,
                    IsActive = x.IsActive ?? false,
                    Title = x.Title,
                    Colleges = x.CampusWalkInColleges.Select(y => new CampusWalkInModel()
                    {
                        CollegeCode = y.CollegeCode,
                        CollegeId = y.CollegeId ?? 0,
                        CollegeName = y.College.CollegeName.Trim(),
                        ExamEndTime = y.ExamEndTime.Value.ToString(),
                        ExamStartTime = y.ExamStartTime.Value.ToString(),
                        StartDateTime= y.StartDateTime,
                    }).Skip(pageStart).Take(pageSize).ToList()
                });
                if (campuses.Any())
                {
                    return campuses.ToList();
                }
                else
                {
                    return new List<CampusGridViewModel>();
                }
            }
        }

        public async Task<IGeneralResult<CampusGridViewModel>> GetCampusByID(int id, int clientId, bool isSuperAdmin)
        {
            IGeneralResult<CampusGridViewModel> result = new GeneralResult<CampusGridViewModel>();
            result.Data = new CampusGridViewModel();
            WalkIn campus = new WalkIn();
            if (isSuperAdmin)
            {
                campus = _context.WalkIns.Include(x => x.State).Include(x => x.Country).Include(x => x.CampusWalkInColleges).Where(x => x.WalkInId == id && x.IsActive == true && x.IsDeleted == false && (clientId==0?true:x.ClientId==clientId)).OrderByDescending(x=>x.WalkInId).FirstOrDefault();
            }
            else
            {
                campus = _context.WalkIns.Include(x => x.State).Include(x => x.Country).Include(x => x.CampusWalkInColleges).Where(x => x.WalkInId == id && x.IsActive == true && x.IsDeleted == false && x.ClientId == clientId).OrderByDescending(x => x.WalkInId).FirstOrDefault();
            }
            if (campus == null)
            {
                result.IsSuccess = false;
                result.Message = "Record Not Found";
                return result;
            }
            var campusGridViewModel = new CampusGridViewModel()
            {
                WalkInID = campus.WalkInId,
                Address1 = campus.Address1,
                Address2 = campus.Address2,
                City = campus.City == null ? "" : campus.City,
                StateID = campus.StateId,
                StateName = campus.StateId > 0 ? campus.State.StateName : "",
                CountryID = campus.CountryId,
                CountryName = campus.CountryId > 0 ? campus.Country.CountryName : "",
                CreatedDate = campus.CreatedDate,
                JobDescription = campus.JobDescription,
                WalkInDate = campus.WalkInDate,
                IsActive = campus.IsActive ?? false,
                Title = campus.Title,
                

               
                Colleges = campus.CampusWalkInColleges.Select(y => new CampusWalkInModel()
                {
                    CampusId= y.CampusId,
                    CollegeCode = y.CollegeCode,
                    CollegeId = y.CollegeId ?? 0,
                   // CollegeName = y.College.CollegeName.Trim(),
                    //CollegeName=y.CollegeId ==8030? y.College.CollegeName:"",
                    ExamEndTime = y.ExamEndTime.Value.ToString(),
                    StartDateTime = y.StartDateTime,
                    ExamStartTime = y.ExamStartTime.Value.ToString()
                }).ToList()
            };
            

            result.Data = campusGridViewModel;
            result.Message = "Record Fatch Successfully";
            result.IsSuccess = true;
            return result;
        }

        public async Task<IGeneralResult<CampusGridViewModel>> UpdateCampusStatus(int id, int clientId,bool status, bool isSuperAdmin)
        {
            IGeneralResult<CampusGridViewModel> result = new GeneralResult<CampusGridViewModel>();
            result.Data = new CampusGridViewModel();
            WalkIn campus = new WalkIn();
            if (isSuperAdmin)
            {
                campus = _context.WalkIns.Where(x => x.WalkInId == id  && x.IsDeleted == false && (clientId==0?true:x.ClientId==clientId)).Include(x => x.State).Include(x => x.Country).Include(x => x.CampusWalkInColleges).FirstOrDefault();
            }
            else
            {
                campus = _context.WalkIns.Where(x => x.WalkInId == id  && x.IsDeleted == false && x.ClientId == clientId).Include(x => x.State).Include(x => x.Country).Include(x => x.CampusWalkInColleges).FirstOrDefault();
            }
            if (campus == null)
            {
                result.IsSuccess = false;
                result.Message = "Record Not Found";
                return result;
            }
            campus.IsActive = status;

            _context.WalkIns.Update(campus);
            int st =  _context.SaveChanges();

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

            var isCountryExist = _context.Countries.Where(w => w.IsDeleted == false).Any(a => a.CountryId == vm.CountryID);
            var allCollages = _context.Colleges.Where(s => s.IsDeleted == false).Select(s => s.CollegeId).ToList();
            var allStates = _context.States.Where(w => w.IsDeleted == false).Select(s => s.StateId).ToList();
            var isStateExist = allStates.Any(a => a == vm.StateID);
            bool isExits = _context.WalkIns.Any(x => x.Title == vm.Title && x.IsDeleted == false);
            if (isExits)
            {
                result.Message = " Title is already exists";
                return result;
            }

            foreach (var clg in vm.Colleges)
            {
                var checkclg = allCollages.Any(s => s == clg.CollegeId);
                if (!checkclg)
                {
                    result.IsSuccess = false;
                    result.Message = "College id " + clg.CollegeId + " is not exist";
                    return result;
                }
                var checkstate = allStates.Any(s => s == clg.StateId);
                if (!checkstate)
                {
                    result.IsSuccess = false;
                    result.Message = "state id " + clg.StateId + " is not exist";
                    return result;
                }
            }

            if (!isCountryExist)
            {
                result.IsSuccess = false;
                result.Message = "Country is not exist";
                return result;
            }
            else if (!isStateExist)
            {
                result.IsSuccess = false;
                result.Message = "State is not exist";
                return result;
            }

            try
            {
                if (vm.WalkInID > 0)
                {
                    WalkIn campus = _context.WalkIns.Where(x => x.WalkInId == vm.WalkInID && (clientId == 0 ? true : x.ClientId == clientId)).Include(x => x.State).Include(x => x.Country).Include(x => x.CampusWalkInColleges).FirstOrDefault();
                    if (campus != null)
                    {
                        campus.WalkInId = vm.WalkInID;
                        campus.WalkInDate = vm.WalkInDate;
                        campus.JobDescription = vm.JobDescription;
                        campus.Address1 = vm.Address1;
                        campus.Address2 = vm.Address2;
                        campus.City = vm.City;
                        campus.StateId = vm.StateID;
                        campus.CountryId = vm.CountryID;
                        campus.Title = vm.Title;
                        campus.CreatedDate = DateTime.Now;
                        _context.Update(campus);

                        if (campus.CampusWalkInColleges != null)
                        {
                            _context.CampusWalkInColleges.RemoveRange(campus.CampusWalkInColleges);
                        }

                        foreach (var rec in vm.Colleges)
                        {
                            if (rec.IsIncludeInWalkIn)
                            {
                                CampusWalkInCollege campusWalkInCollege = new CampusWalkInCollege()
                                {
                                    WalkInId = campus.WalkInId,
                                    CollegeId = rec.CollegeId,
                                    ExamStartTime = TimeSpan.Parse(rec.ExamStartTime),
                                    ExamEndTime = TimeSpan.Parse(rec.ExamEndTime),
                                    CollegeCode = rec.CollegeCode,
                                    StartDateTime = rec.StartDateTime,
                                    
                                    IsCompleted = null
                                };
                                _context.CampusWalkInColleges.Update(campusWalkInCollege);
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

                var sv = new WalkIn()
                {
                    WalkInId=vm.WalkInID,
                    WalkInDate = vm.WalkInDate,
                    JobDescription = vm.JobDescription,
                    Address1 = vm.Address1,
                    Address2 = vm.Address2,
                    City = vm.City,
                    StateId = vm.StateID,
                    CountryId = vm.CountryID,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    Title = vm.Title,
                    ClientId = clientId == 0 ? 0 : clientId
                };
                _context.WalkIns.Update(sv);
                _context.SaveChanges();

                foreach (var rec in vm.Colleges)
                {
                    if (rec.IsIncludeInWalkIn)
                    {
                        CampusWalkInCollege campusWalkInCollege = new CampusWalkInCollege()
                        {
                            WalkInId = sv.WalkInId,
                            CollegeId = rec.CollegeId,
                            ExamStartTime = TimeSpan.Parse(rec.ExamStartTime),
                            ExamEndTime = TimeSpan.Parse(rec.ExamEndTime),
                            CollegeCode = rec.CollegeCode,
                            StartDateTime = rec.StartDateTime,
                            IsCompleted = null
                        };
                        _context.CampusWalkInColleges.Update(campusWalkInCollege);
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

        public async Task<IGeneralResult<CampusGridViewModel>> DeleteCampus(int id, int clientId, bool isDelete, bool isSuperAdmin)
        {
            IGeneralResult<CampusGridViewModel> result = new GeneralResult<CampusGridViewModel>();
            result.Data = new CampusGridViewModel();
            WalkIn campus = new WalkIn();
            if (isSuperAdmin)
                campus = _context.WalkIns.Where(x => x.WalkInId == id && x.IsActive == true && x.IsDeleted == false && (clientId==0?true:x.ClientId==clientId)).Include(x => x.State).Include(x => x.Country).Include(x => x.CampusWalkInColleges).FirstOrDefault();
            else
                campus = _context.WalkIns.Where(x => x.WalkInId == id && x.IsActive == true && x.IsDeleted == false && x.ClientId == clientId).Include(x => x.State).Include(x => x.Country).Include(x => x.CampusWalkInColleges).FirstOrDefault();

            if (campus == null)
            {
                result.IsSuccess = false;
                result.Message = "Record Not Found";
                return result;
            }
            campus.IsDeleted = isDelete;

            _context.WalkIns.Update(campus);
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