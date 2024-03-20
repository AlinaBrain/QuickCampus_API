 using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Services
{
   public class CountryService : BaseRepository<BtprojecQuickcampusContext, MstCityStateCountry>, ICountryRepo
    {
        private readonly BtprojecQuickcampusContext _context;
        public CountryService(BtprojecQuickcampusContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CountryVM>> GetAllCountries()
        {

            var countries = _context.MstCityStateCountries.Where(x => x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.CountryName).Select(x => new CountryModel() { CountryID = x.CountryId, CountryName = x.CountryName }).ToList();
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
