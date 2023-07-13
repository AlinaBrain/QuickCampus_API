using QuickCampus_Core.ViewModel;

namespace QuickCampus_Core.Interfaces
{
    public interface IQuestion 
    {
        Task<List<QuestionViewModelAdmin>> GetAllQuestion();
        Task<QuestionViewModelAdmin> GetQuestionById(int QuestionId);
        Task<List<QuestionTypeViewModelAdmin>> GetAllQuestionType();
        Task<List<SectionViewModelAdmin>> GetAllSection();
        Task<List<GroupViewModelAdmin>> GetAllGroups();
    }
}
