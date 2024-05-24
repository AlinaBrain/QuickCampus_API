using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Text.Json;

namespace QuickCampus_Core.Services
{
    public class ClientRepo : BaseRepository<BtprojecQuickcampustestContext, TblClient>, IClientRepo
    {
        private readonly BtprojecQuickcampustestContext _context;
        private readonly IConfiguration _config;

        public ClientRepo(BtprojecQuickcampustestContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
       

        public string GetClientRoleName(int ClientId)
        {
            string RoleName = "";
            int UserId = _context.TblUsers.Where(x => x.ClientId == ClientId).FirstOrDefault().Id;
            if(UserId  > 0)
            {
                var r = _context.TblUserRoles.Include(y=>y.Role).Where(x => x.UserId == UserId).FirstOrDefault();
                if (r != null)
                {
                    RoleName = _context.TblRoles.Where(x=>x.Id==r.RoleId).First().Name;
                }
            }
            return RoleName;
        }

        public string GetClientAppRoleName(int ClientId)
        {
            string RoleName = "";
            int UserId = _context.TblUsers.Where(x => x.ClientId == ClientId).FirstOrDefault().Id;
            if (UserId > 0)
            {
                RoleName = _context.TblUserAppRoles.Include(y => y.Role).Where(x => x.UserId == UserId).FirstOrDefault().Role.AppRoleName;
            }
            return RoleName;
        }
        //public IGeneralResult<string> GetLoggedInUserMenu(int loggedInUser)
        //{
        //    GeneralResult<string> res = new GeneralResult<string>();
        //    try
        //    {
        //        if (loggedInUser > 0)
        //        {
        //            TblMenuItemUserPermission data = new TblMenuItemUserPermission();
        //            var getRole = _context.TblMenuItemUserPermissions.Where(x=>x.UserId == loggedInUser).FirstOrDefault();
        //            var menuList = JsonSerializer.Deserialize<MenuRoleVm>(getRole != null ? getRole.Items  : "");

        //            if (menuList.MenuItem.Count > 0)
        //            {
        //                res.IsSuccess = true;
        //                res.Message = "Role fetched Successfully";
        //                res.Data = menuList

        //            }
        //            else
        //            {
        //                res.Message = "Something went wrong!";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Message = "server error";
        //    }
        //    return res;
        //}
    }
}
