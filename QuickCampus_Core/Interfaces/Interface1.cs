using QuickCampus_DAL.Context;

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
