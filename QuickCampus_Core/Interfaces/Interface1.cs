using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Interfaces
{
    public interface IApplicantRepository

    {
        Task<Applicant> GetAsync(int? categoryId);

        Task<List<Applicant>> GetAllAsync();

        Task<Applicant> CreateAsync(Applicant applicant);

        Task DeleteAsync(int ApplicantId);

        Task UpdateAsync(Applicant applicant);
    }
}
