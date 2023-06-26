using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class CountryStateVM
    {
        public CountryStateVM() 
        { 
            this.Countries = new List<SelectListItem>();
            this.States = new List<SelectListItem>();
        }
       public List<SelectListItem> Countries { get; set; }
       public List<SelectListItem> States { get;set; }

        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }


    }
}
