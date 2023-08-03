using QuickCampus_Core.Common;
using QuickCampus_Core.ViewModel;

namespace QuickCampus_Core.Interfaces
{
    public interface IQuestion
    {
        Task<IGeneralResult<List<QuestionViewModelAdmin>>> GetAllQuestion(int clientId, bool isSuperAdmin);
        Task<IGeneralResult<QuestionViewModelAdmin>> GetQuestionById(int QuestionId, int clientid, bool issuperadmin);
        Task<List<GroupViewModelAdmin>> GetAllGroups(bool isSuperAdmin, int clientId);
        Task<IGeneralResult<string>> ActiveInactiveQuestion(int questionId, int clientId, bool isSuperAdmin, bool isActive);
        Task<List<QuestionTypeViewModelAdmin>> GetAllQuestionTypes(bool isSuperAdmin, int clientId);
        Task<List<SectionViewModelAdmin>> GetAllSectionList(bool isSuperAdmin, int clientId);
        Task<IGeneralResult<string>> DeleteQuestion(int questionId, int clientId, bool isSuperAdmin, bool isDelete);
        Task<IGeneralResult<string>> AddQuestion(QuestionViewModelAdmin model,bool isSuperAdmin);
        Task<IGeneralResult<string>> UpdateQuestion(QuestionViewModelAdmin model, bool isSuperAdmin);
    }
}
