using Microsoft.EntityFrameworkCore;
using QuickCampus_Core.Interfaces;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QuickCampus_Core.Services
{
    public class ClientRepo : BaseRepository<QuikCampusDevContext, TblClient>, IClientRepo
    {
        //public static Task<bool> UsernameExistsAsync(string name)
        //{
        //    throw new NotImplementedException();
        //}
    }



}
