using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Text.Json;

namespace QuickCampus_Core.Services
{
    public class ClientRepo : BaseRepository<BtprojecQuickcampusContext, TblClient>, IClientRepo
    {
        private readonly BtprojecQuickcampusContext _context;
        private readonly IConfiguration _config;

        public ClientRepo(BtprojecQuickcampusContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        public  IGeneralResult<string> AddMenuRoles(MenuRoleVm vm, int loggedInUser)
        {
            GeneralResult<string> res = new GeneralResult<string>();
            try
            {
                if (vm != null)
                {
                    TblMenuItemUserPermission data = new TblMenuItemUserPermission();
                    data.UserId = loggedInUser;
                    data.Items = JsonSerializer.Serialize(vm.MenuItem);
                    data.CreatedAt = DateTime.Now;
                    data.CreatedBy = vm.UserId;
                    var saveRole = _context.TblMenuItemUserPermissions.Add(data);
                    _context.SaveChanges();
                    if (saveRole.Entity.Id > 0)
                    {
                        res.IsSuccess = true;
                        res.Message = "Role Added Successfully";
                    }
                    else
                    {
                        res.Message = "Something went wrong!";
                    }
                }
            }
            catch(Exception ex)
            {
                res.Message = "server error";
            }
            return res;
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
