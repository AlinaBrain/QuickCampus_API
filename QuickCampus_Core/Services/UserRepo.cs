using Microsoft.Extensions.Configuration;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Services
{
    public class UserRepo : BaseRepository<QuikCampusDevContext, TblUser>, IUserRepo
    {
        public Task Add(TblClient tblClient)
        {
            throw new NotImplementedException();
        }
    }
}
