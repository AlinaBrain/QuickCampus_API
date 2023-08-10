using Microsoft.Extensions.Configuration;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Services
{
    public class UserRepo : BaseRepository<QuikCampusDevContext, TblUser>, IUserRepo
    {
        private readonly QuikCampusDevContext _context;
        private IConfiguration _config;

        public UserRepo(QuikCampusDevContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        public Task Add(TblClient tblClient)
        {
            throw new NotImplementedException();
        }

        public async Task<IGeneralResult<string>> DeleteRole(bool isDeleted, int id, int clientid, bool isSuperAdmin)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            TblUser rl = new TblUser();
            if (isSuperAdmin)
            {
                rl = _context.TblUsers.Where(w => w.IsDelete == false && (clientid == 0 ? true : w.ClientId == clientid) && w.Id == id).FirstOrDefault();
            }
            else
            {
                rl = _context.TblUsers.Where(w => w.IsDelete == false && w.ClientId == clientid && w.Id==id).FirstOrDefault();
            }
            if (rl == null)
            {
                result.IsSuccess = false;
                result.Message = "Role not found";
                return result;
            }

            rl.IsDelete = isDeleted;
            rl.IsActive = false;
            dbContext.TblUsers.Update(rl);
            int a = dbContext.SaveChanges();
            if (a > 0)
            {
                result.IsSuccess = true;
                result.Message = "User delete successfully";
                return result;

            }
            else
            {
                result.IsSuccess = false;
                result.Message = "something went wrong";
                return result;
            }
        }

        public async Task<IGeneralResult<string>> ActiveInActiveRole(bool isActive, int id, int clientid, bool isSuperAdmin)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            TblUser rl = new TblUser();
            if (isSuperAdmin)
            {
                rl = _context.TblUsers.Where(w => w.IsDelete == false && (clientid == 0 ? true : w.ClientId == clientid) && w.Id== id).FirstOrDefault();
            }
            else
            {
                rl = _context.TblUsers.Where(w => w.IsDelete == false && w.ClientId == clientid && w.Id==id).FirstOrDefault();
            }
            if (rl == null)
            {
                result.IsSuccess = false;
                result.Message = "User not found";
                return result;
            }

            rl.IsActive = isActive;
            dbContext.TblUsers.Update(rl);
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
