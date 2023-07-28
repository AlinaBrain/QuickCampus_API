using QuickCampus_Core.Common;
using QuickCampus_Core.ViewModel;

namespace QuickCampus_Core.Interfaces
{
    public interface IQuestion 
    {
        Task<IGeneralResult<List<QuestionViewModelAdmin>>> GetAllQuestion(int clientId,bool isSuperAdmin);
        Task<QuestionViewModelAdmin> GetQuestionById(int QuestionId);
        Task<List<QuestionTypeViewModelAdmin>> GetAllQuestionType();
        Task<List<SectionViewModelAdmin>> GetAllSection();
        Task<List<GroupViewModelAdmin>> GetAllGroups();
        Task<IGeneralResult<string>> ActiveInactiveQuestion(int questionId);
    }
}
