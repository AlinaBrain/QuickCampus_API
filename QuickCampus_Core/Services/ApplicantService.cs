using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_DAL.Context;
using static QuickCampus_Core.ViewModel.ApplicantViewModel;

namespace QuickCampus_Core.Services
{
    public class ApplicantRepoServices : BaseRepository<BtprojecQuickcampusContext, Applicant>, IApplicantRepo
    {
        private readonly BtprojecQuickcampusContext _context;
        private IConfiguration _config;

        public ApplicantRepoServices(BtprojecQuickcampusContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        public async Task<IGeneralResult<string>> ActiveInActiveRole(bool isActive, int id, int clientid, bool isSuperAdmin)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            Applicant rl = new Applicant();
            if (isSuperAdmin)
            {
                rl = _context.Applicants.Where(w => w.IsDeleted == false && (clientid == 0 ? true : w.ClientId == clientid)).FirstOrDefault();
            }
            else
            {
                rl = _context.Applicants.Where(w => w.IsDeleted == false && (w.ClientId == clientid)).FirstOrDefault();
            }
            if (rl == null)
            {
                result.IsSuccess = false;
                result.Message = "Applicant not found";
                return result;
            }

            rl.IsActive = isActive;
            dbContext.Applicants.Update(rl);
            int a = dbContext.SaveChanges();
            if (a > 0)
            {
                result.IsSuccess = true;
                result.Message = "status update successfully";
                return result;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "something went wrong";
                return result;
            }
        }

        public ApplicantGridViewModel GetApplicantByID(int id)
        {
            var applicant = dbContext.Applicants.Where(x => x.ApplicantId == id).FirstOrDefault();
            return applicant != null ? new ApplicantGridViewModel()
            {
                ApplicantID = applicant.ApplicantId,
              
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                EmailAddress = applicant.EmailAddress,
                PhoneNumber = applicant.PhoneNumber,
                HighestQualification = applicant.HigestQualification,
                HighestQualificationPercentage = applicant.HigestQualificationPercentage,
                MatricPercentage = applicant.MatricPercentage,
                IntermediatePercentage = applicant.IntermediatePercentage,
                Skills = applicant.Skills,
                StatusID = applicant.StatusId ?? 0,
                Comment = applicant.Comment,
               
            } : new ApplicantGridViewModel();

        }

        //public ApplicantGridViewModel UpdateApplicant(ApplicantGridViewModel model)
        //{
           
        //        Applicant applicant = dbContext.Applicants.Where(x => x.ApplicantId == model.ApplicantID).FirstOrDefault();
        //        if (applicant != null)
        //        {
                   
        //            applicant.FirstName = model.FirstName;
        //            applicant.LastName = model.LastName;
        //            applicant.EmailAddress = model.EmailAddress;
        //            applicant.PhoneNumber = model.PhoneNumber;
        //            applicant.HighestQualification = model.HighestQualification;
        //            applicant.HighestQualificationPercentage = model.HighestQualificationPercentage;
        //            applicant.MatricPercentage = model.MatricPercentage;
        //            applicant.IntermediatePercentage = model.IntermediatePercentage;
        //            applicant.Skills = model.Skills;
        //            applicant.StatusId = model.StatusID;
        //            applicant.Comment = model.Comment;
        //            if (model.CompanyId > 0)
        //            {
        //                applicant.AssignedToCompany = model.CompanyId;
        //            }
        //        }
        //        var result = dbContext.Entry(applicant).State = EntityState.Modified;
        //        dbContext.SaveChanges();
        //        if (result > 0)
        //        {
        //            return new GeneralResult()
        //            {
        //                Successful = true,
        //                Message = "Applicant has been updated successfully.",
        //                Value = applicant.ApplicantId
        //            };
        //        }
        //        else
        //        {
        //            return new GeneralResult()
        //            {
        //                Successful = false,
        //                Message = "Applicant has not been updated.",
                        
        //            };
        //        }
            
        //}
        public async Task<IGeneralResult<string>> DeleteApplicant(bool isDeleted, int id, int clientid, bool isSuperAdmin)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            Applicant applicant = new Applicant();
            if (isSuperAdmin)
            {
                applicant = _context.Applicants.Where(w => w.IsDeleted == false && (clientid == 0 ? true : w.ClientId == clientid) && w.ApplicantId == id).FirstOrDefault();
            }
            else
            {
                applicant = _context.Applicants.Where(w => w.IsDeleted == false && w.ClientId == clientid && w.ApplicantId == id).FirstOrDefault();
            }
            if (applicant == null)
            {
                result.IsSuccess = false;
                result.Message = "Role not found";
                return result;
            }

            applicant.IsDeleted = isDeleted;
            applicant.IsActive = false;
            dbContext.Applicants.Update(applicant);
            int a = dbContext.SaveChanges();
            if (a > 0)
            {
                result.IsSuccess = true;
                result.Message = "Applicant delete successfully";
                return result;

            }
            else
            {
                result.IsSuccess = false;
                result.Message = "something went wrong";
                return result;
            }
           
        }



    }
}
