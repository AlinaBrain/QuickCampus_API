using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuickCampus_Core.Common;
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
   public class CountryService : BaseRepository<QuikCampusContext, Country>, ICountryRepo
    {
        private readonly QuikCampusContext _context;
        public CountryService(QuikCampusContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CountryVM>> GetAllCountries()
        {

            var countries = _context.Countries.Where(x => x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.CountryName).Select(x => new CountryModel() { CountryID = x.CountryId, CountryName = x.CountryName }).ToList();
            if (countries.Any())
            {
                return new IGeneralResult()
                {
                    IsSuccess = true,
                    Message = "Countries has fetched successfully.",
                    Value = countries
                };
            }
            else
            {
                return new IGeneralResult()
                {
                    IsSuccess = false,
                    Message = "No data found.",
                    Value = null
                };
            }
            

        }
    }
}
