using Microsoft.EntityFrameworkCore;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static QuickCampus_Core.ViewModel.ApplicantViewModel;

namespace QuickCampus_Core.Services
{
    public class ApplicantRepoServices : BaseRepository<QuikCampusContext, Applicant>, IApplicantRepo
    {
       
        public async Task<IEnumerable<Applicant>> GetApplicant()
        {

            var a = await (from f in dbContext.Applicants.AsQueryable() select f).ToListAsync();
            return await (from f in dbContext.Applicants.AsQueryable() select f).ToListAsync();
        }

       
        public Task Update(IEnumerable<Applicant> applicantdetail)
        {
            throw new NotImplementedException();
        }
        public ApplicantGridViewModel GetApplicantByID(int id)
        {
            var applicant = dbContext.Applicants.Where(x => x.ApplicantId == id).FirstOrDefault();
            return applicant != null ? new ApplicantGridViewModel()
            {
                ApplicantID = applicant.ApplicantId,
                ApplicantToken = applicant.ApplicantToken,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                EmailAddress = applicant.EmailAddress,
                PhoneNumber = applicant.PhoneNumber,
                HigestQualification = applicant.HigestQualification,
                HigestQualificationPercentage = applicant.HigestQualificationPercentage,
                MatricPercentage = applicant.MatricPercentage,
                IntermediatePercentage = applicant.IntermediatePercentage,
                Skills = applicant.Skills,
                StatusID = applicant.StatusId ?? 0,
                Comment = applicant.Comment,
                RegisteredDate = applicant.RegisteredDate != null ? applicant.RegisteredDate.Value.ToShortDateString() : "",
            } : new ApplicantGridViewModel();

        }

        public ApplicantGridViewModel UpdateApplicant(ApplicantGridViewModel model)
        {
           
                Applicant applicant = dbContext.Applicants.Where(x => x.ApplicantId == model.ApplicantID).FirstOrDefault();
                if (applicant != null)
                {
                    applicant.ApplicantToken = model.ApplicantToken;
                    applicant.FirstName = model.FirstName;
                    applicant.LastName = model.LastName;
                    applicant.EmailAddress = model.EmailAddress;
                    applicant.PhoneNumber = model.PhoneNumber;
                    applicant.HigestQualification = model.HigestQualification;
                    applicant.HigestQualificationPercentage = model.HigestQualificationPercentage;
                    applicant.MatricPercentage = model.MatricPercentage;
                    applicant.IntermediatePercentage = model.IntermediatePercentage;
                    applicant.Skills = model.Skills;
                    applicant.StatusId = model.StatusID;
                    applicant.Comment = model.Comment;
                    if (model.CompanyId > 0)
                    {
                        applicant.AssignedToCompany = model.CompanyId;
                    }
                }
                var result = dbContext.Entry(applicant).State = EntityState.Modified;
                dbContext.SaveChanges();
                if (result > 0)
                {
                    return new GeneralResult()
                    {
                        Successful = true,
                        Message = "Applicant has been updated successfully.",
                        Value = applicant.ApplicantId
                    };
                }
                else
                {
                    return new GeneralResult()
                    {
                        Successful = false,
                        Message = "Applicant has not been updated.",
                        
                    };
                }
            
        }


        void IApplicantRepo.Update(IEnumerable<Applicant> applicantdetail)
        {
            throw new NotImplementedException();
        }

       
        //public async Task<IEnumerable<Applicant>> AddAsync(ApplicantViewModel applicantViewModel)
        //{
        //    return await (from f in dbContext.Applicants.AsQueryable() select f).ToListAsync();
        //}
        public Task SaveChangesAsync(ApplicantViewModel applicantViewModel)
        {
            throw new NotImplementedException();
        }

        public void Update(ApplicantGridViewModel applicantdetail)
        {
            
        }
        Task<Applicant> IApplicantRepo.CreateAsync(Applicant applicant)
        {
            throw new NotImplementedException();
        }

        public Task<Applicant> GetAsync(int? categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Applicant>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int ApplicantId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Applicant applicant)
        {
            throw new NotImplementedException();
        }

        Task IApplicantRepo.AddAsync(ApplicantViewModel applicantViewModel)
        {
            return (from f in dbContext.Applicants.AsQueryable() select f).ToListAsync();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public void Add(ApplicantViewModel applicantViewModel)
        {
            throw new NotImplementedException();
        }

        void IApplicantRepo.UpdateApplicant(ApplicantGridViewModel model)
        {
            Applicant applicant = dbContext.Applicants.Where(x => x.ApplicantId == model.ApplicantID).FirstOrDefault();
            if (applicant != null)
            {
                applicant.ApplicantToken = model.ApplicantToken;
                applicant.FirstName = model.FirstName;
                applicant.LastName = model.LastName;
                applicant.EmailAddress = model.EmailAddress;
                applicant.PhoneNumber = model.PhoneNumber;
                applicant.HigestQualification = model.HigestQualification;
                applicant.HigestQualificationPercentage = model.HigestQualificationPercentage;
                applicant.MatricPercentage = model.MatricPercentage;
                applicant.IntermediatePercentage = model.IntermediatePercentage;
                applicant.Skills = model.Skills;
                applicant.StatusId = model.StatusID;
                applicant.Comment = model.Comment;
                if (model.CompanyId > 0)
                {
                    applicant.AssignedToCompany = model.CompanyId;
                }
            }
            var result = dbContext.Entry(applicant).State = EntityState.Modified;
            dbContext.SaveChanges();
            //if (result > 0)
            //{
            //    return new GeneralResult()
            //    {
            //        Successful = true,
            //        Message = "Applicant has been updated successfully.",
            //        Value = applicant.ApplicantId
            //    };
            //}
            //else
            //{
            //    return new GeneralResult()
            //    {
            //        Successful = false,
            //        Message = "Applicant has not been updated.",

            //    };
            //}

        }

        public IEnumerable<object> GetAllApplicant()
        {
            throw new NotImplementedException();
        }



        //public ApplicantViewModel Add(ApplicantViewModel applicantViewModel)
        //{
        //        dbContext.Set<Applicant>().Add(applicantViewModel);
        //        dbContext.SaveChangesAsync();
        //        return applicantViewModel;
        //}
        //public async Task<IGeneralResult<string>> AddCommentOrEditComments(ApplicantViewModel dto)
        //{
        //    var oldValue = dbContext.Applicants.Where(w => w.ApplicantId == dto.ApplicantID ).FirstOrDefault();
        //    GeneralResult<string> result = new GeneralResult<string>();
        //    ApplicantViewModel res = new ApplicantViewModel();

        //    if (dto.ApplicantID > 0)
        //    {
        //        {
        //           var  applicantdata = dbContext.Applicants.Where(w => w.ApplicantId == dto.ApplicantID).FirstOrDefault();
        //        }

        //        applicantdata.FirstName = dto.FirstName;
        //        applicantdata.LastName = dto.LastName;
        //        applicantdata.EmailAddress = dto.EmailAddress;

        //    }

        //    dto.ApplicantID > 0 ? dbContext.Update(dto) : await dbContext.Applicants.AddAsync(dto);
        //    int save = await dbContext.SaveChangesAsync();
        //    if (save > 0)
        //    {
        //        if (dto.ApplicantID > 0)
        //        {
        //            var newdata = dbContext.Applicants.Where(w => w.ApplicantId == dto.ApplicantID ).FirstOrDefault();

        //            dbContext.Update(dto);
        //        }
        //        result.IsSuccess = true;
        //        result.Message = "Comment  is Added Successfully";
        //    }
        //    else
        //    {
        //        result.IsSuccess = false;
        //        result.Message = "Something Went Wrong";
        //    }

        //    return result;
        //}
    }
}
