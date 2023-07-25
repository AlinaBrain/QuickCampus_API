using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<CampusGridViewModel>> GetAllCampus()
        {
            var campuses = _context.WalkIns.Include("State").Include("Country").Where(x => x.IsDeleted == false).OrderByDescending(x => x.WalkInDate).Select(x => new CampusGridViewModel()
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

        public async Task<CampusGridViewModel> GetCampusByID(int id)
        {
            var campus =  _context.WalkIns.Where(x => x.WalkInId == id && x.IsActive == true && x.IsDeleted == false).Include(x=>x.State).Include(x=>x.Country).Include(x=>x.CampusWalkInColleges).FirstOrDefault();

            CampusGridViewModel campusGridViewModel = new CampusGridViewModel()
            {
                WalkInID = campus.WalkInId,
                Address1 = campus.Address1,
                Address2 = campus.Address2, 
                City = campus.City==null?"": campus.City,
                StateID = campus.State.StateId,
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

            return campusGridViewModel;


        }

        Task<WalkIn> IGenericRepository<WalkIn>.Add(WalkIn entity)
        {
            throw new NotImplementedException();
        }

        Task IGenericRepository<WalkIn>.AddApplicantAsync(ApplicantViewModel model)
        {
            throw new NotImplementedException();
        }

        bool IGenericRepository<WalkIn>.Any(Expression<Func<WalkIn, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Task<bool> IGenericRepository<WalkIn>.AnyAsync(Expression<Func<WalkIn, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Task IGenericRepository<WalkIn>.Delete(WalkIn entity)
        {
            throw new NotImplementedException();
        }

        WalkIn IGenericRepository<WalkIn>.FirstOrDefault(Expression<Func<WalkIn, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Task<WalkIn> IGenericRepository<WalkIn>.FirstOrDefaultAsync(Expression<Func<WalkIn, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Task<List<WalkIn>> IGenericRepository<WalkIn>.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<List<WalkIn>> IGenericRepository<WalkIn>.GetAll(Expression<Func<WalkIn, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        IQueryable<WalkIn> IGenericRepository<WalkIn>.GetAllQuerable()
        {
            throw new NotImplementedException();
        }

        Task<WalkIn> IGenericRepository<WalkIn>.GetById(int Id)
        {
            throw new NotImplementedException();
        }

        Task IGenericRepository<WalkIn>.Save()
        {
            throw new NotImplementedException();
        }


    }
}
