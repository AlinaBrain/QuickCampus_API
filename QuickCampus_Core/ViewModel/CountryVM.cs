using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class CountryVM
    {
        public static explicit operator CountryVM(MstCityStateCountry items)
        {
            return new CountryVM
            {
                //ClientId = items.ClientId,
                CountryId = items.CountryId,
                CountryName = items.CountryName,
                //IsActive = items.IsActive,
                //IsDeleted = items.IsDeleted,
            };
        }
        public int CountryId { get; set; }

        public string? CountryName { get; set; }

        //public bool? IsActive { get; set; }

        //public bool? IsDeleted { get; set; }

        //public int? ClientId { get; set; }

        public MstCityStateCountry ToCountryDbModel()
        {
            return new MstCityStateCountry
            {
                CountryName = CountryName,
                IsActive = true,
                IsDeleted = false,
                //ClientId = ClientId
            };
        }
        public MstCityStateCountry ToUpDateDbModel()
        {
            return new MstCityStateCountry
            {   CountryId = CountryId,
                CountryName = CountryName,
                IsActive = true,
                IsDeleted = false,
                //ClientId = ClientId
            };
        }
    }
}
