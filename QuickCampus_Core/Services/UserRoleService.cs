using Microsoft.EntityFrameworkCore;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Linq.Expressions;

namespace QuickCampus_Core.Services
{
    public class UserRoleService: IUserRoleRepo
    {
        private readonly BtprojecQuickcampustestContext _context;
        public UserRoleService(BtprojecQuickcampustestContext context)
        {
            _context = context;
        }

        public async Task<IGeneralResult<string>> SetClientAdminRole(int userId)
        {
            IGeneralResult<string> result= new GeneralResult<string>();

            if (userId != 0)
            {
                TblUserRole role = new TblUserRole()
                {
                    RoleId = 1,
                    UserId = userId
                };
                _context.TblUserRoles.Add(role);
                int a =  _context.SaveChanges();

                if(a > 0)
                {
                    result.IsSuccess = true;
                }
                return result;
            }
            return result;
        }
    }
}
