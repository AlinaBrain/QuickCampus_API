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
    public class StateServices :  IStateRepo
    {
        private readonly QuikCampusDevContext _context;
        public StateServices(QuikCampusDevContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CountryVM>> GetStateByCountryID(int countryID)
        {
               var states = _context.States.Where(x => x.IsActive == true && x.IsDeleted == false && x.CountryId == countryID).OrderBy(x => x.StateName).Select(x => new StateModel() { StateID = x.StateId, StateName = x.StateName, CountryID = x.CountryId ?? 0 }).ToList();

                if (states.Any())
                {
                    return new IGeneralResult()
                    {
                        IsSuccess = true,
                        Message = "List of all States of selected country.",
                        Value = null
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
