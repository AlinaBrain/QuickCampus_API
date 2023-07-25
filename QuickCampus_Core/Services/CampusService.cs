using Microsoft.EntityFrameworkCore;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Linq.Expressions;

namespace QuickCampus_Core.Services
{
    public class CampusService : BaseRepository<QuikCampusDevContext, WalkIn>, ICampusRepo
    {
        private readonly QuikCampusDevContext _context;
        public CampusService(QuikCampusDevContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<CampusGridViewModel>> Add(CampusGridViewModel campusGridViewModel)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CampusGridViewModel>> GetAllCampus(int clientId)
        {
            var campuses = _context.WalkIns.Where(x => x.IsDeleted == false && (clientId==0?true: x.ClientId == clientId)).Include(x=>x.State).Include(x=>x.Country).OrderByDescending(x => x.WalkInDate).Select(x => new CampusGridViewModel()
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

                }).ToList()
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

        public async Task<IGeneralResult<CampusGridViewModel>> GetCampusByID(int id, int clientId)
        {
            IGeneralResult<CampusGridViewModel> result = new GeneralResult<CampusGridViewModel>();
            result.Data = new CampusGridViewModel();
            WalkIn campus = new WalkIn();
            if (clientId == 0)
                campus = _context.WalkIns.Where(x => x.WalkInId == id && x.IsActive == true && x.IsDeleted == false).Include(x => x.State).Include(x => x.Country).Include(x => x.CampusWalkInColleges).FirstOrDefault();
            else
                campus = _context.WalkIns.Where(x => x.WalkInId == id && x.IsActive == true && x.IsDeleted == false && x.ClientId == clientId).Include(x => x.State).Include(x => x.Country).Include(x => x.CampusWalkInColleges).FirstOrDefault();

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
                    CollegeCode = y.CollegeCode,
                    CollegeId = y.CollegeId ?? 0,
                    CollegeName = y.College.CollegeName,
                    ExamEndTime = y.ExamEndTime.Value.ToString(),
                    ExamStartTime = y.ExamStartTime.Value.ToString()
                }).ToList()
            };

            result.Data = campusGridViewModel;
            result.Message = "Record Fatch Successfully";
            result.IsSuccess = true;
            return result;
        }
    }
}