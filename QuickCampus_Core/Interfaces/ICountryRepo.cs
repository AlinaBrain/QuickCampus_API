using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Interfaces
{
    public interface ICountryRepo : IGenericRepository<Country>
    {
        Task<IEnumerable<CountryVM>> GetAllCountries();
    }
}
