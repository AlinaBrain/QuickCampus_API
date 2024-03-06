using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class CompanyVm 
    {
        public static explicit operator CompanyVm(Company items)
        {
            return new CompanyVm
            {
                CompanyId=items.CompanyId,
                CompanyName=items.CompanyName,
                IsActive=items.IsActive,
                Isdeleted=items.Isdeleted,
                CreatedDate=items.CreatedDate,
                ModifiedDate=items.ModifiedDate,
            };
        }
        public int CompanyId { get; set; }
        [Required(ErrorMessage = "Company Name is required.")]
        [MaxLength(50, ErrorMessage = "Name must be at most 20 characters long.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]*$", ErrorMessage = "Only alphabetic characters are allowed in the name.")]
        public string? CompanyName { get; set; }

        public bool? IsActive { get; set; }

        public bool? Isdeleted { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public  Company ToDbModel()
        {
            return new Company
            {
                CompanyName = CompanyName,
                IsActive = true,
                Isdeleted=false,
                CreatedDate = DateTime.Now,
                ModifiedDate =CompanyId>0? DateTime.Now:null,
            };
        }
        
    }
}
