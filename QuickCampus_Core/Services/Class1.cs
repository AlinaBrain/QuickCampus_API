using QuickCampus_Core.Interfaces;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Services
{
    public class ApplicantRepository : IApplicantRepository
    {
        private readonly QuikCampusContext _context;

        public ApplicantRepository(QuikCampusContext context)
        {
            this._context = context;
        }

        Task<Applicant> IApplicantRepository.CreateAsync(Applicant applicant)
        {
            throw new NotImplementedException();
        }

        Task IApplicantRepository.DeleteAsync(int ApplicantId)
        {
            throw new NotImplementedException();
        }

        Task<List<Applicant>> IApplicantRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<Applicant> IApplicantRepository.GetAsync(int? categoryId)
        {
            throw new NotImplementedException();
        }

        Task IApplicantRepository.UpdateAsync(Applicant applicant)
        {
            throw new NotImplementedException();
        }
    }
}
