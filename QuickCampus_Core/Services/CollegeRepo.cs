using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Services
{
    public class CollegeRepo : BaseRepository<QuikCampusDevContext, College>, ICollegeRepo
    {
        private readonly QuikCampusDevContext _context;
        private IConfiguration _config;

        public CollegeRepo(QuikCampusDevContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        public async Task<IGeneralResult<string>> DeleteCollege(int id, int clientid, bool isSuperAdmin)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            College college = new College();
            if (isSuperAdmin)
            {
                college = _context.Colleges.Where(w => w.IsDeleted == false && (clientid == 0 ? true : w.ClientId == clientid) && w.CollegeId == id).FirstOrDefault();
            }
            else
            {
                college = _context.Colleges.Where(w => w.IsDeleted == false && w.ClientId == clientid && w.CollegeId == id).FirstOrDefault();
            }
            if (college == null)
            {
                result.IsSuccess = false;
                result.Message = "College not found";
                return result;
            }

            college.IsDeleted = true;
            college.IsActive = false;
            college.ModifiedDate = DateTime.Now;
            dbContext.Colleges.Update(college);
            int a = dbContext.SaveChanges();
            if (a > 0)
            {
                result.IsSuccess = true;
                result.Message = "College delete successfully";
                return result;

            }
            else
            {
                result.IsSuccess = false;
                result.Message = "something went wrong";
                return result;
            }
        }
    }
}

