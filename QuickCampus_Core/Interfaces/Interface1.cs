using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Interfaces
{
    public interface IApplicantRepository

    {
        Task<TblApplicant> GetAsync(int? categoryId);

        Task<List<TblApplicant>> GetAllAsync();

        Task<TblApplicant> CreateAsync(TblApplicant applicant);

        Task DeleteAsync(int ApplicantId);

        Task UpdateAsync(TblApplicant applicant);
    }
}
