using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static QuickCampus_Core.ViewModel.ApplicantViewModel;

namespace QuickCampus_Core.Interfaces
{
    public interface IApplicantRepo : IGenericRepository<Applicant>
    {
        Task AddAsync(ApplicantViewModel applicantViewModel);

        //List<Applicant> GetApplicant(int StatusId);
        Task<IEnumerable<Applicant>> GetApplicant();
        
       
        ApplicantGridViewModel GetApplicantByID(int id);
        Task SaveChangesAsync(ApplicantViewModel applicantViewModel);
        void Update(IEnumerable<Applicant> applicantdetail);
        void Update(ApplicantGridViewModel applicantdetail);
        Task<Applicant> GetAsync(int? categoryId);

        Task<List<Applicant>> GetAllAsync();

        Task<Applicant> CreateAsync(Applicant applicant);

        Task DeleteAsync(int ApplicantId);

        Task UpdateAsync(Applicant applicant);
        Task SaveChangesAsync();
        void Add(ApplicantViewModel applicantViewModel);
        void UpdateApplicant(ApplicantGridViewModel applicantDetail);
        IEnumerable<object> GetAllApplicant();
    }
}
