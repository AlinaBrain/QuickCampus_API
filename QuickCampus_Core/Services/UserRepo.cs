using Microsoft.Extensions.Configuration;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Services
{
    public class UserRepo : BaseRepository<BtprojecQuickcampustestContext, TblUser>, IUserRepo
    {
        private readonly BtprojecQuickcampustestContext _context;
        private IConfiguration _config;

        public UserRepo(BtprojecQuickcampustestContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
       
    }
}
