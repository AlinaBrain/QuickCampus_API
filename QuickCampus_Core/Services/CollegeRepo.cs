using Microsoft.EntityFrameworkCore;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Services
{
    public class CollegeRepo : BaseRepository<QuikCampusDevContext, College>, ICollegeRepo
    {
        public  CollegeVM GetCollegeByID(int id)
        {
            using (var context = new QuikCampusDevContext())
            {
                var college = context.Colleges.Where(x => x.CollegeId == id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                return college != null ? new CollegeVM()
                {
                    CollegeID = college.CollegeId,
                    CollegeName = college.CollegeName,
                    Logo = college.Logo,
                    Address1 = college.Address1,
                    Address2 = college.Address2,
                    City = college.City,
                    StateID = college.StateId,
                    StateName = college.StateId > 0 ? college.State.StateName : "",
                    CountryID = college.CountryId,
                    CountryName = college.CountryId > 0 ? college.Country.CountryName : "",
                    CreatedDate = college.CreatedDate,
                    CreatedBy = college.CreatedBy ?? 0,
                    IsActive = college.IsActive ?? false,
                    ContectPhone = college.ContectPhone,
                    ContectPerson = college.ContectPerson,
                    ContectEmail = college.ContectEmail
                } : new CollegeVM();
            }
        }

        public async Task <IEnumerable<CollegeVM>> GetAllCollege()
        {
            using (var context = new QuikCampusDevContext())
            {
                var colleges = await dbContext.Colleges.Where(x => x.IsDeleted == false).OrderBy(x => x.CollegeName).Select(x => new CollegeVM()
                {
                    CollegeID = x.CollegeId,
                    CollegeName = x.CollegeName,
                    Address1 = x.Address1,
                    Address2 = x.Address2,
                    City = x.City,
                    StateID = x.StateId,
                    StateName = x.StateId > 0 ? x.State.StateName : "",
                    CountryID = x.CountryId,
                    CountryName = x.CountryId > 0 ? x.Country.CountryName : "",
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive ?? false,
                    ContectEmail = x.ContectEmail == null ? "---" : x.ContectEmail,
                    ContectPerson = x.ContectPerson == null ? "---" : x.ContectPerson,
                    ContectPhone = x.ContectPhone == null ? "---" : x.ContectPhone

                }).ToListAsync();
                if (colleges.Any())
                {
                    return colleges.ToList();
                }
                else
                {
                    return new List<CollegeVM>();
                }
            }
        }
    }
    }

