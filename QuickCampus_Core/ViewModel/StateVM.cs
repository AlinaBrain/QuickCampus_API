using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class StateVM
    {
        public static explicit operator StateVM(MstCity_State items)
        {
            return new StateVM
            {
                StateId = items.StateId,
                StateName = items.StateName,
                CountryId = items.CountryId,
                IsActive = items.IsActive,
                IsDeleted   = items.IsDeleted,
                ClientId = items.ClientId,
            };
        }
        public int? StateId { get; set; }

        public string? StateName { get; set; }

        public int? CountryId { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }
        public int? ClientId { get; set; }


        public MstCity_State ToStateDbModel()
        {
            return new MstCity_State
            {
                StateName = StateName,
                CountryId = CountryId,
                IsDeleted = false,
                IsActive=true,
                ClientId= ClientId,
            };
        }
        public MstCity_State ToUpdateDbModel()
        {
            return new MstCity_State
            {
                StateId = (int)StateId,
                StateName = StateName,
                IsActive = true,
                IsDeleted = false,
                CountryId = CountryId,
                ClientId=ClientId
                
            };
        }
    }
}
