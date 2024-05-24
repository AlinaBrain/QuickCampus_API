using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class CityVm
    {
        public static explicit operator CityVm(MstCity items)
        {
            return new CityVm
            {
                CityId = items.CityId,
                StateId = items.StateId,
                CityName = items.CityName,
                IsActive = items.IsActive,
                IsDeleted = items.IsDeleted,
                ClientId = items.ClientId,
                UpdatedAt = items.UpdatedAt,
                CreatedAt = items.CreatedAt,

            };
        }
        public int CityId { get; set; }

        public string? CityName { get; set; }

        public int? StateId { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool? IsDeleted { get; set; }

        public int? ClientId { get; set; }


        public MstCity ToCityDbModel()
        {
            return new MstCity
            {
                CityName = CityName,
                ClientId = ClientId,
                StateId= StateId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = CityId > 0 ? DateTime.UtcNow : null,
                IsActive = true,
                IsDeleted= false,
            };

        }
        public MstCity ToUpdateDbModel()
        {
            return new MstCity
            {
                CityId= CityId,
                CityName = CityName,
                ClientId = ClientId,
                UpdatedAt = DateTime.Now,
                CreatedAt = CityId > 0 ? DateTime.UtcNow : null,
                IsActive = true,
                IsDeleted = false,


            };
        }


    }
}
