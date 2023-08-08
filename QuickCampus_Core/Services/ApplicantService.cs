using Microsoft.Extensions.Configuration;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_DAL.Context;


namespace QuickCampus_Core.Services
{
    public class ApplicantRepoServices : BaseRepository<QuikCampusDevContext, Applicant>, IApplicantRepo
    {
        private readonly QuikCampusDevContext _context;
        private IConfiguration _config;

        public ApplicantRepoServices(QuikCampusDevContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        public async Task<IGeneralResult<string>> ActiveInActiveRole(bool isActive, int id, int clientid, bool isSuperAdmin)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            Applicant rl = new Applicant();
            if (isSuperAdmin)
            {
                rl = _context.Applicants.Where(w => w.IsDeleted == false && (clientid == 0 ? true : w.ClientId == clientid)).FirstOrDefault();
            }
            else
            {
                rl = _context.Applicants.Where(w => w.IsDeleted == false && w.ClientId == clientid).FirstOrDefault();
            }
            if (rl == null)
            {
                result.IsSuccess = false;
                result.Message = "Applicant not found";
                return result;
            }

            rl.IsActive = isActive;
            dbContext.Applicants.Update(rl);
            int a = dbContext.SaveChanges();
            if (a > 0)
            {
                result.IsSuccess = true;
                result.Message = "status update successfully";
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
