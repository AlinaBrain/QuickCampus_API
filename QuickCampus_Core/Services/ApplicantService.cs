using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_DAL.Context;
using static QuickCampus_Core.ViewModel.ApplicantViewModel;

namespace QuickCampus_Core.Services
{
    public class ApplicantRepoServices : BaseRepository<BtprojecQuickcampustestContext, Applicant>, IApplicantRepo
    {
        private readonly BtprojecQuickcampustestContext _context;
        private IConfiguration _config;

        public ApplicantRepoServices(BtprojecQuickcampustestContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
    }
}
