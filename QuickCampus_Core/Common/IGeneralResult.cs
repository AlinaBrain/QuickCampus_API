using QuickCampus_Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Common
{
    public class GeneralResult<T> : IGeneralResult<T>
    {
        public bool IsSuccess { get; set; }
        public bool IsActive { get; set; }

        public string Message { get; set; }
        public T Data { get; set; }
    }
    public interface IGeneralResult<T>
    {
        bool IsSuccess { get; set; }
        string Message { get; set; }
        T Data { get; set; }

    }

    //public static IGeneralResult GetAllCountries()
    //{

    //    var countries = _context.Countries.Where(x => x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.CountryName).Select(x => new CountryModel() { CountryID = x.CountryId, CountryName = x.CountryName }).ToList();
    //    if (countries.Any())
    //    {
    //        return new GeneralResult()
    //        {
    //            Successful = true,
    //            Message = "Countries has fetched successfully.",
    //            Value = countries
    //        };
    //    }
    //    else
    //    {
    //        return new GeneralResult()
    //        {
    //            Successful = false,
    //            Message = "No data found.",
    //            Value = null
    //        };
    //    }


    //}
}
