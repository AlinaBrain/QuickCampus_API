using Microsoft.Extensions.Configuration;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Services
{
    public class CityServices :BaseRepository<BtprojecQuickcampustestContext,MstCity>,ICityRepo
    {
        private readonly BtprojecQuickcampustestContext _context;
        private IConfiguration _config;

        public CityServices(BtprojecQuickcampustestContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
       
    }
}
