 using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Services
{
   public class CountryService : BaseRepository<BtprojecQuickcampustestContext, MstCityStateCountry>, ICountryRepo
    {
        private readonly BtprojecQuickcampustestContext _context;
        public CountryService(BtprojecQuickcampustestContext context)
        {
            _context = context;
        }
    }
}
