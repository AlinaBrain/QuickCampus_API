using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QuickCampus_Core.Services
{
    public class CampusService : BaseRepository<QuikCampusContext, WalkIn>, ICampusRepo
    {
        private readonly QuikCampusContext _context;
        public CampusService(QuikCampusContext context)
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

        public async Task<IEnumerable<CampusGridViewModel>> GetCampusByID(int id)
        {
            var campus = _context.WalkIns.Where(x => x.WalkInId == id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
            return (IEnumerable<CampusGridViewModel>)(campus != null ? new CampusGridViewModel()
            {
                WalkInID = campus.WalkInId,
                WalkInDate = campus.WalkInDate,
                JobDescription = campus.JobDescription,
                Address1 = campus.Address1,
                Address2 = campus.Address2,
                City = campus.City,
                StateID = campus.StateId,
                StateName = campus.StateId > 0 ? campus.State.StateName : "",
                CountryID = campus.CountryId,
                CountryName = campus.CountryId > 0 ? campus.Country.CountryName : "",
                CreatedDate = campus.CreatedDate,
                CreatedBy = campus.CreatedBy ?? 0,
                //WalkInStartTime = campus.CampusWalkInColleges.FirstOrDefault() != null ? campus.CampusWalkInColleges.FirstOrDefault().StartDateTime.ToString() : "",
                IsActive = campus.IsActive ?? false,
                Title = campus.Title,
                Colleges = campus.CampusWalkInColleges != null ? campus.CampusWalkInColleges.Select(walkincolleges => new CampusWalkInModel()
                {
                    CollegeId = walkincolleges.CollegeId ?? 0,
                    CollegeName = walkincolleges.College.CollegeName,
                    IsIncludeInWalkIn = true,
                    ExamEndTime = walkincolleges.ExamEndTime.ToString(),
                    ExamStartTime = walkincolleges.ExamStartTime.ToString(),
                    CollegeCode = walkincolleges.CollegeCode,
                    StartDateTime = walkincolleges.StartDateTime

                }).ToList() : new List<CampusWalkInModel>()
            } : new CampusGridViewModel());
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

        Task IGenericRepository<WalkIn>.Update(WalkIn entity)
        {
            throw new NotImplementedException();
        }
       
        
    }
}
        