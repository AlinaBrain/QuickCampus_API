using Microsoft.EntityFrameworkCore;
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
    public class CollegeRepo : BaseRepository<QuikCampusDevContext, College>, ICollegeRepo
    {
        public  CollegeGridViewModel GetCollegeByID(int id)
        {
            using (var context = new QuikCampusDevContext())
            {
                var college = context.Colleges.Where(x => x.CollegeId == id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                return college != null ? new CollegeGridViewModel()
                {
                    CollegeID = college.CollegeId,
                    CollegeName = college.CollegeName,
                    LogoImage = college.Logo,
                    Address1 = college.Address1,
                    Address2 = college.Address2,
                    City = college.City,
                    StateID = college.StateId,
                    StateName = college.StateId > 0 ? college.State.StateName : "",
                    CountryID = college.CountryId,
                    CountryName = college.CountryId > 0 ? college.Country.CountryName : "",
                    CreatedDate = college.CreatedDate,
                    CreatedBy = college.CreatedBy ?? 0,
                    IsActive = college.IsActive ?? false,
                    ContectPhone = college.ContectPhone,
                    ContectPerson = college.ContectPerson,
                    ContectEmail = college.ContectEmail
                } : new CollegeGridViewModel();
            }
        }

        //public static IGeneralResult AddCollege(CollegeGridViewModel model)
        //{
        //    using (var context = new QuikCampusDevContext())
        //    {
        //        College college = new College()
        //        {
        //            CollegeName = model.CollegeName,
        //            Address1 = model.Address1,
        //            Address2 = model.Address2,
        //            City = model.City,
        //            StateId = model.StateID,
        //            CountryId = model.CountryID,
        //            Logo = model.LogoImage,
        //            IsActive = true,
        //            IsDeleted = false,
        //            CreatedBy = 1,
        //            CreatedDate = DateTime.Now,
        //            ContectEmail = model.ContectEmail,
        //            ContectPerson = model.ContectPerson,
        //            ContectPhone = model.ContectPhone
        //        };
        //        var res = context.Colleges.Add(college);
        //        context.SaveChanges();
        //        if (res.CollegeId > 0)
        //        {
        //            return new GeneralResult()
        //            {
        //                Successful = true,
        //                Message = "College has been created successfully.",
        //                Value = res.CollegeId
        //            };
        //        }
        //        else
        //        {
        //            return new GeneralResult()
        //            {
        //                Successful = false,
        //                Message = "College has not been created.",
        //                Value = null
        //            };
        //        }
        //    }
        //}

        /// <summary>
        /// This method is to update college entity
        /// </summary>
        /// <param name="model"></param>
        /// <returns>object of General Result</returns>
        //public static IGeneralResult UpdateCollege(CollegeGridViewModel model)
        //{
        //    using (var context = new QuikCampusEntities())
        //    {
        //        College college = context.Colleges.Where(x => x.CollegeId == model.CollegeID).FirstOrDefault();
        //        if (college != null)
        //        {
        //            college.CollegeName = model.CollegeName;
        //            college.Address1 = model.Address1;
        //            college.Address2 = model.Address2;
        //            college.City = model.City;
        //            college.StateId = model.StateID;
        //            college.CountryId = model.CountryID;
        //            if (model.LogoImage != null)
        //                college.Logo = model.LogoImage;
        //            college.ContectPhone = model.ContectPhone;
        //            college.ContectPerson = model.ContectPerson;
        //            college.ContectEmail = model.ContectEmail;
        //        }
        //        var result = context.Entry(college).State = System.Data.Entity.EntityState.Modified;
        //        int status = context.SaveChanges();
        //        if (status > 0)
        //        {
        //            return new GeneralResult()
        //            {
        //                Successful = true,
        //                Message = "College has been updated successfully.",
        //                Value = college.CollegeId
        //            };
        //        }
        //        else
        //        {
        //            return new GeneralResult()
        //            {
        //                Successful = false,
        //                Message = "College has not been updated.",
        //                Value = null
        //            };
        //        }
        //    }
        //}

        public async Task <IEnumerable<CollegeGridViewModel>> GetAllCollege()
        {
            using (var context = new QuikCampusDevContext())
            {
                var colleges = await dbContext.Colleges.Where(x => x.IsDeleted == false).OrderBy(x => x.CollegeName).Select(x => new CollegeGridViewModel()
                {
                    CollegeID = x.CollegeId,
                    CollegeName = x.CollegeName,
                    Address1 = x.Address1,
                    Address2 = x.Address2,
                    City = x.City,
                    StateID = x.StateId,
                    StateName = x.StateId > 0 ? x.State.StateName : "",
                    CountryID = x.CountryId,
                    CountryName = x.CountryId > 0 ? x.Country.CountryName : "",
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive ?? false,
                    ContectEmail = x.ContectEmail == null ? "---" : x.ContectEmail,
                    ContectPerson = x.ContectPerson == null ? "---" : x.ContectPerson,
                    ContectPhone = x.ContectPhone == null ? "---" : x.ContectPhone

                }).ToListAsync();
                if (colleges.Any())
                {
                    return colleges.ToList();
                }
                else
                {
                    return new List<CollegeGridViewModel>();
                }
            }
        }
    }
    }

