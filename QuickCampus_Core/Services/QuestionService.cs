using Microsoft.EntityFrameworkCore;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.Services
{
    public class QuestionService : IQuestion
    {
        private readonly QuikCampusDevContext _context;
        public QuestionService(QuikCampusDevContext context)
        {
            _context = context;
        }
        public async Task<IGeneralResult<List<QuestionViewModelAdmin>>> GetAllQuestion(int clientid, bool issuperadmin)
        {
            IGeneralResult<List<QuestionViewModelAdmin>> result = new GeneralResult<List<QuestionViewModelAdmin>>();

            List<QuestionViewModelAdmin> record = new List<QuestionViewModelAdmin>();

            if (issuperadmin)
            {
                result.Data = _context.Questions.Where(x => x.IsDeleted == false && x.IsActive == true && (clientid == 0 ? true : x.ClentId == clientid)).Select(x => new QuestionViewModelAdmin()
                {
                    QuestionId = x.QuestionId,
                    QuestionTypeName = x.QuestionType.QuestionType1,
                    QuestionSection = x.Section.Section1,
                    QuestionGroup = x.Group.GroupName,
                    Question = x.Text,
                    IsActive = x.IsActive ?? false
                }).ToList();
                if (result.Data.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Record Fetch Successfully";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Record Not Found";
                }
                return result;
            }
            result.Data = _context.Questions.Where(x => x.IsDeleted == false && x.IsActive == true && x.ClentId == clientid).Select(x => new QuestionViewModelAdmin()
            {
                QuestionId = x.QuestionId,
                QuestionTypeName = x.QuestionType.QuestionType1,
                QuestionSection = x.Section.Section1,
                QuestionGroup = x.Group.GroupName,
                Question = x.Text,
                IsActive = x.IsActive ?? false
            }).ToList();

            if (result.Data.Count > 0)
            {
                result.IsSuccess = true;
                result.Message = "Record Fatch Successfully";
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Record Not Found";
            }
            return result;

        }
        public async Task<QuestionViewModelAdmin> GetQuestionById(int QuestionId)
        {
            var questions = await _context.Questions.Where(x => x.IsDeleted == false && x.IsActive == true && x.QuestionId == QuestionId).Select(x => new QuestionViewModelAdmin()
            {
                QuestionId = x.QuestionId,
                QuestionTypeName = x.QuestionType.QuestionType1,
                QuestionTypeId = x.QuestionTypeId ?? 0,
                SectionId = x.SectionId ?? 0,
                GroupId = x.GroupId ?? 0,
                QuestionSection = x.Section.Section1,
                Question = x.Text,
                Marks = x.Marks ?? 0,
                options = x.QuestionOptions.Select(y => new OptionViewModelAdmin()
                {
                    OptionId = y.OptionId,
                    OptionText = y.OptionText,
                    OptionImage = y.OptionImage,
                    IsCorrect = y.IsCorrect ?? false,
                    IsNew = false
                }).ToList()

            }).FirstOrDefaultAsync();
            if (questions != null)
            {
                return questions;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<QuestionTypeViewModelAdmin>> GetAllQuestionType()
        {
            var questionsType = await _context.QuestionTypes.Select(x => new QuestionTypeViewModelAdmin()
            {
                Questiontype = x.QuestionType1,
                QuestionTypeId = x.QuestionTypeId

            }).ToListAsync();
            if (questionsType.Any())
            {
                return questionsType.ToList();
            }
            else
            {
                return new List<QuestionTypeViewModelAdmin>();
            }
        }
        public async Task<List<SectionViewModelAdmin>> GetAllSection()
        {
            var sections = await _context.Sections.Select(x => new SectionViewModelAdmin()
            {
                SectionName = x.Section1,
                SectionId = x.SectionId

            }).ToListAsync();

            return sections;
        }
        public async Task<List<GroupViewModelAdmin>> GetAllGroups()
        {
            var Groups = await _context.Groupdls.Select(x => new GroupViewModelAdmin()
            {
                GroupName = x.GroupName,
                GroupId = x.GroupId
            }).ToListAsync();
            if (Groups.Any())
            {
                return Groups;
            }
            else
            {
                return new List<GroupViewModelAdmin>();
            }
        }

        public async Task<IGeneralResult<string>> ActiveInactiveQuestion(int questionId)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            var question  = _context.Questions.Where(x => x.QuestionId == questionId).FirstOrDefault();

            if (question != null)
            {
                if (question.IsActive == true)
                {
                    question.IsActive = false;
                }
                else
                {
                    question.IsActive = true;
                }
                _context.Questions.Update(question);
                _context.SaveChanges();


                result.IsSuccess = true;
                result.Message = "Question status has been changed sucessfully.";
                
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Question status not changed. Please try again.";
            }
            return result;
        }
    }
}