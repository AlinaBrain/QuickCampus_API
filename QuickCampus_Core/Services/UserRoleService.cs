using Microsoft.EntityFrameworkCore;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Linq.Expressions;

namespace QuickCampus_Core.Services
{
    public class UserRoleService: BaseRepository<BtprojecQuickcampustestContext, TblUserRole>,IUserRoleRepo
    {
       
    }
}
