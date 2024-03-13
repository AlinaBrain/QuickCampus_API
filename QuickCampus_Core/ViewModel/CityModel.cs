using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class CityModel
    {
        public int CityId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
       
        public string? CityName { get; set; }

        public int? StateId { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool? IsDeleted { get; set; }

        public int? ClientId { get; set; }
    }
}
