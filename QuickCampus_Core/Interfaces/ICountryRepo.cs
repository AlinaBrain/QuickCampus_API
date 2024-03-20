using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Interfaces
{
    public interface ICountryRepo : IGenericRepository<MstCity_State_Country>
    {
        Task<IEnumerable<CountryVM>> GetAllCountries();
    }
}
