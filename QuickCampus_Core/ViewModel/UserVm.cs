using Microsoft.AspNetCore.Mvc;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class UserVm
    {

        //public static explicit operator UserVm(TblUser item)
        //{
        //    return new UserVm
        //    {
        //        Id = item.Id,
        //        TemplateName = item.TemplateName,
        //        SubjectName = item.SubjectName,
        //        Description = item.Body,
        //    };

        //}
        //public int Id { get; set; }
        //[Required(ErrorMessage = "Template name is required."), MaxLength(30)]
        //[Remote("PageExist", "Templates", AdditionalFields = "Id", ErrorMessage = ("Page already exist!"))]
        //[RegularExpression(@"^([a-zA-Z]).+", ErrorMessage = "Starting with Alphabates only. Atleast two characters required.")]
        //public string TemplateName { get; set; }
        //[Required(ErrorMessage = "Subject Name is Required."), MaxLength(50)]
        //[RegularExpression(@"^([a-zA-Z]).+", ErrorMessage = "Starting with Alphabates only. Atleast two characters required.")]
        //public string SubjectName { get; set; }

        //[Required(ErrorMessage = "Description Name is required."), MinLength(10)]
        //public string Description { get; set; }
        //public DateTime? UpdatedAt { get; set; }
        //public bool? Active { get; set; }
        //public DateTime? DeletedAt { get; set; }

        //public TblUser ToTemplateDBModel()
        //{
        //    return new TblUser
        //    {
        //        Id = Id,
        //        TemplateName = TemplateName,
        //        SubjectName = SubjectName,
        //        Body = Description,
        //        Active = true,
        //        CreatedAt = DateTime.UtcNow,
        //        UpdatedAt = Id > 0 ? DateTime.UtcNow : null,
        //        DeletedAt = DeletedAt,
        //    };
        //}
    }
}
