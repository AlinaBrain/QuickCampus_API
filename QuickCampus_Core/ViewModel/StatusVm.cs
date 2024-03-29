using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class StatusVm
    {
        public static explicit operator StatusVm(Status items)
        {
            return new StatusVm
            {
                StatusId = items.StatusId,
                StatusName = items.StatusName,
                
            };

        }
        public int StatusId { get; set; }
        
        public string? StatusName { get; set; }
        public Status ToDbModel()
        {
            return new Status
            {
                StatusName = StatusName,
                IsActive = true,
                IsDeleted=false,
                CreatedDate = DateTime.Now,
                ModifiedDate= StatusId> 0? DateTime.Now:null
            };
        }
    }
}
