using QuickCampus_Core.Common;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
