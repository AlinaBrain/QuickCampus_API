﻿using QuickCampus_Core.Interfaces;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Services
{
    public class UserRepo : BaseRepository<QuikCampusDevContext, TblUser>, IUserRepo
    {
        
    }
}
