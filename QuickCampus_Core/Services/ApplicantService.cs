using Microsoft.Extensions.Configuration;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_DAL.Context;


namespace QuickCampus_Core.Services
{
    public class ApplicantRepoServices : BaseRepository<QuikCampusDevContext, Applicant>, IApplicantRepo
    {
        private readonly QuikCampusDevContext _context;
        private IConfiguration _config;

        public ApplicantRepoServices(QuikCampusDevContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        public async Task<IGeneralResult<string>> ActiveInActiveRole(bool isActive, int id, int clientid, bool isSuperAdmin)
        {
            throw new NotImplementedException();
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
                HigestQualification = applicant.HigestQualification,
                HigestQualificationPercentage = applicant.HigestQualificationPercentage,
                MatricPercentage = applicant.MatricPercentage,
                IntermediatePercentage = applicant.IntermediatePercentage,
                Skills = applicant.Skills,
                StatusID = applicant.StatusId ?? 0,
                Comment = applicant.Comment,
               
            } : new ApplicantGridViewModel();

        }

        public ApplicantGridViewModel UpdateApplicant(ApplicantGridViewModel model)
        {
           
                Applicant applicant = dbContext.Applicants.Where(x => x.ApplicantId == model.ApplicantID).FirstOrDefault();
                if (applicant != null)
                {
                   
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
        }

        public IEnumerable<object> GetAllApplicant()
        {
            throw new NotImplementedException();
        }

    }
}
