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
        public async Task<IGeneralResult<string>> DeleteCity(bool isDeleted, int id, int clientid, bool isSuperAdmin)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            MstCity city = new MstCity();
            if (isSuperAdmin)
            {
                city = _context.MstCities.Where(w => w.IsDeleted == false && (clientid == 0 ? true : w.ClientId == clientid) && w.CityId == id).FirstOrDefault();
            }
            else
            {
                city = _context.MstCities.Where(w => w.IsDeleted == false && w.ClientId == clientid && w.CityId == id).FirstOrDefault();
            }
            if (city == null)
            {
                result.IsSuccess = false;
                result.Message = "College not found";
                return result;
            }

            city.IsDeleted = isDeleted;
            city.IsActive = false;
            dbContext.MstCities.Update(city);
            int a = dbContext.SaveChanges();
            if (a > 0)
            {
                result.IsSuccess = true;
                result.Message = "MstCity delete successfully";
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
