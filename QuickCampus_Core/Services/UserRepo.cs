using Microsoft.EntityFrameworkCore;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Services
{
    public class UserRepo : BaseRepository<QuikCampusDevContext, TblUser>, IUserRepo
    {
        public async Task<List<TblUser>> getUserclient()
        {
            var query = from u in dbContext.TblUsers
                        join r in dbContext.TblRoles on u.ClientId equals r.ClientId
                        join rp in dbContext.TblRolePermissions on r.Id equals rp.RoleId
                        where u.ClientId == 2
                        select new TblUser {Name= u.Name, Email=u.Email, Mobile = u.Mobile, ClientId = u.ClientId };
                      List<TblUser> resultList = query.ToList();
            return resultList;
        }
        public List<RolePermissions> getPermission(int roleId, TblRole tblRole)
        {
            List<RolePermissions> rolePermissions = new List<RolePermissions>();

            rolePermissions = dbContext.TblRolePermissions.Include(i => i.Permission).Where(w => w.RoleId == roleId).Select(s => new RolePermissions()
            {
                Id = s.Id,
                PermissionName = s.Permission.PermissionName,
                DisplayName = s.Permission.PermissionDisplay
            }).ToList();

            return rolePermissions;
        }
    }
}
