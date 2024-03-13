using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using QuickCampus_Core.Common;
using QuickCampus_Core.Common.Helper;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Collections;
using System.Linq.Expressions;

namespace QuickCampus_Core.Services
{
    public class QuestionService : BaseRepository<QuikCampusDevContext, Question>, IQuestion
    {
        private readonly IConfiguration _config;
        private readonly QuikCampusDevContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly string basepath;
        private string baseUrl;
        private readonly ProcessUploadFile _processUploadFile;

        public QuestionService(QuikCampusDevContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ProcessUploadFile processUploadFile)
        {
            _config = config;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            baseUrl = _config.GetSection("APISitePath").Value;
            _processUploadFile = processUploadFile;
        }
        public async Task<IGeneralResult<List<QuestionViewModelAdmin>>> GetAllQuestion(int clientid, bool issuperadmin, int pageStart, int pageSize)
        {
            IGeneralResult<List<QuestionViewModelAdmin>> result = new GeneralResult<List<QuestionViewModelAdmin>>();
            List<QuestionViewModelAdmin> record = new List<QuestionViewModelAdmin>();
            if (issuperadmin)
            {
                var totalCount = result.Data = _context.Questions.Where(x => x.IsDeleted == false && x.IsActive==true &&(clientid == 0 ? true : x.ClentId == clientid)).Select(x => new QuestionViewModelAdmin()
                {
                    QuestionId = x.QuestionId,
                    GroupId=(int)x.GroupId,
                    QuestionTypeName = x.QuestionType.QuestionType1,
                    QuestionSection = x.Section.Section1,
                    QuestionGroup = x.Group.GroupName,
                    Text = x.Text,
                    Marks=Convert.ToInt32(x.Marks),
                    IsActive = x.IsActive ?? false
                }).ToList();
                result.Data = _context.Questions.Where(x => x.IsDeleted == false &&x.IsActive==true && (clientid == 0 ? true : x.ClentId == clientid)).Select(x => new QuestionViewModelAdmin()
                {
                    QuestionId = x.QuestionId,
                    QuestionTypeId=(int)x.QuestionTypeId,
                    QuestionTypeName = x.QuestionType.QuestionType1,
                    QuestionSection = x.Section.Section1,
                    QuestionGroup = x.Group.GroupName,
                    Text = x.Text,
                    Marks=x.Marks,
                    GroupId=(int)x.GroupId,
                    SectionId=(int)x.SectionId,
                    IsActive = x.IsActive ?? false
                }).OrderByDescending(x => x.QuestionId).Skip(pageStart).Take(pageSize).ToList();
                if (result.Data.Count > 0 && result.Data.Any(x=>x.IsActive==true))
                {
                    result.IsSuccess = true;
                    result.Message = "Record Fetch Successfully";
                    result.TotalRecordCount = result.Data.Count();
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Record Not Found";
                }
                return result;
            }
            if (clientid == 0)
            {
                result.IsSuccess = false;
                result.Message = "Invalid ClientId";
                return result;
            }

            var totalCountWithClientId = result.Data = _context.Questions.Where(x => x.IsDeleted == false && x.ClentId == clientid).Select(x => new QuestionViewModelAdmin()
            {
                QuestionId = x.QuestionId,
                QuestionTypeName = x.QuestionType.QuestionType1,
                QuestionSection = x.Section.Section1,
                QuestionGroup = x.Group.GroupName,
                Text = x.Text,
                IsActive = x.IsActive ?? false
            }).ToList();
            result.Data = _context.Questions.Where(x => x.IsDeleted == false && x.ClentId == clientid).Select(x => new QuestionViewModelAdmin()
            {
                QuestionId = x.QuestionId,
                QuestionTypeName = x.QuestionType.QuestionType1,
                QuestionSection = x.Section.Section1,
                QuestionGroup = x.Group.GroupName,
                Text = x.Text,
                IsActive = x.IsActive ?? false
            }).Skip(pageStart).Take(pageSize).ToList();

            if (result.Data.Count > 0)
            {
                result.IsSuccess = true;
                result.Message = "Record Fatch Successfully";
                result.TotalRecordCount = totalCountWithClientId.Count();
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Record Not Found";
            }
            return result;

        }
        public async Task<IGeneralResult<QuestionViewModelAdmin>> GetQuestionById(int QuestionId, int clientid, bool issuperadmin)
        {
            IGeneralResult<QuestionViewModelAdmin> res = new GeneralResult<QuestionViewModelAdmin>();
            res.Data = new QuestionViewModelAdmin();
            if (issuperadmin)
            {

                var question = _context.Questions.Include(x => x.QuestionOptions).Where(x => x.IsDeleted == false && x.IsActive == true && x.QuestionId == QuestionId && (clientid == 0 ? true : x.ClentId == clientid)).Select(x => new QuestionViewModelAdmin()
                {
                    QuestionId = x.QuestionId,
                    QuestionTypeName = x.QuestionType.QuestionType1,
                    QuestionTypeId = x.QuestionTypeId ?? 0,
                    SectionId = x.SectionId ?? 0,
                    GroupId = x.GroupId ?? 0,
                    QuestionSection = x.Section.Section1,
                    Text = x.Text,
                    IsActive = true,
                    IsDeleted = false,

                    Marks = x.Marks ?? 0,
                    options = x.QuestionOptions.Select(y => new OptionViewModelAdmin()
                    {
                        QuestionId = x.QuestionId,
                        OptionId = y.OptionId,
                        OptionText = y.OptionText,
                        OptionImage = y.OptionImage,
                        IsCorrect = y.IsCorrect ?? false,
                        IsNew = false
                    }).ToList()

                }).FirstOrDefault();

                if (question == null)
                {
                    res.IsSuccess = false;
                    res.Message = "Question is not Active Succefully";
                    res.Data = null;
                    return res;
                }
                res.IsSuccess = true;
                res.Message = "Question fatch Successfully";
                res.Data = question;
                return res;
            }
            if (clientid == 0)
            {
                res.IsSuccess = false;
                res.Message = "Invalid Client";
                res.Data = null;
                return res;
            }

            var question1 = _context.Questions.Include(x => x.QuestionOptions).Where(x => x.IsDeleted == false && x.IsActive == true && x.QuestionId == QuestionId && x.ClentId == clientid).Select(x => new QuestionViewModelAdmin()
            {
                QuestionId = x.QuestionId,
                QuestionTypeName = x.QuestionType.QuestionType1,
                QuestionTypeId = x.QuestionTypeId ?? 0,
                SectionId = x.SectionId ?? 0,
                GroupId = x.GroupId ?? 0,
                QuestionSection = x.Section.Section1,
                Text = x.Text,
                Marks = x.Marks ?? 0,
                options = x.QuestionOptions.Select(y => new OptionViewModelAdmin()
                {
                    OptionId = y.OptionId,
                    OptionText = y.OptionText,
                    OptionImage = y.OptionImage,
                    IsCorrect = y.IsCorrect ?? false,
                    IsNew = false
                }).ToList()
            }).FirstOrDefault();
            if (question1 == null)
            {
                res.IsSuccess = false;
                res.Message = "question not found";
                res.Data = null;
                return res;
            }
            res.IsSuccess = true;
            res.Message = "Question fetch successfully";
            res.Data = question1;
            return res;
        }
        public async Task<List<GroupViewModelAdmin>> GetAllGroups(bool isSuperAdmin, int clientId)
        {
            List<GroupViewModelAdmin> groupViewModelAdmin = new List<GroupViewModelAdmin>();
            if (isSuperAdmin)
            {
                groupViewModelAdmin = _context.Groupdls.Where(w => (clientId == 0 ? true : w.ClentId == clientId)).Select(x => new GroupViewModelAdmin()
                {
                    GroupName = x.GroupName,
                    GroupId = x.GroupId
                }).ToList();

                if (groupViewModelAdmin.Count > 0)
                {
                    return groupViewModelAdmin;
                }
                else
                {
                    return new List<GroupViewModelAdmin>();
                }
            }

            if (clientId == 0)
            {
                return new List<GroupViewModelAdmin>();
            }
            groupViewModelAdmin = _context.Groupdls.Where(w => w.ClentId == clientId).Select(x => new GroupViewModelAdmin()
            {
                GroupName = x.GroupName,
                GroupId = x.GroupId
            }).ToList();

            if (groupViewModelAdmin.Count > 0)
            {
                return groupViewModelAdmin;
            }
            else
            {
                return new List<GroupViewModelAdmin>();
            }
        }
        public async Task<List<QuestionTypeViewModelAdmin>> GetAllQuestionTypes(bool isSuperAdmin, int clientId)
        {
            List<QuestionTypeViewModelAdmin> questionViewModelAdmin = new List<QuestionTypeViewModelAdmin>();
            if (isSuperAdmin)
            {
                questionViewModelAdmin = _context.QuestionTypes.Where(w => (clientId == 0 ? true : w.ClentId == clientId)).Select(x => new QuestionTypeViewModelAdmin()
                {
                    Questiontype = x.QuestionType1,
                    QuestionTypeId = x.QuestionTypeId
                }).ToList();

                if (questionViewModelAdmin.Count > 0)
                {
                    return questionViewModelAdmin;
                }
                else
                {
                    return new List<QuestionTypeViewModelAdmin>();
                }
            }
            if (clientId == 0)
            {
                return new List<QuestionTypeViewModelAdmin>();
            }
            questionViewModelAdmin = _context.QuestionTypes.Where(w => w.ClentId == clientId).Select(x => new QuestionTypeViewModelAdmin()
            {
                Questiontype = x.QuestionType1,
                QuestionTypeId = x.QuestionTypeId
            }).ToList();


            if (questionViewModelAdmin.Count > 0)
            {
                return questionViewModelAdmin;
            }
            else
            {
                return new List<QuestionTypeViewModelAdmin>();
            }
        }
        public async Task<List<SectionViewModelAdmin>> GetAllSectionList(bool isSuperAdmin, int clientId)
        {
            List<SectionViewModelAdmin> sectionViewModelAdmin = new List<SectionViewModelAdmin>();
            if (isSuperAdmin)
            {
                sectionViewModelAdmin = _context.Sections.Where(w => (clientId == 0 ? true : w.ClentId == clientId)).Select(x => new SectionViewModelAdmin()
                {
                    SectionName = x.Section1,
                    SectionId = x.SectionId
                }).ToList();

                if (sectionViewModelAdmin.Count > 0)
                {
                    return sectionViewModelAdmin;
                }
                else
                {
                    return new List<SectionViewModelAdmin>();
                }
            }
            if (clientId == 0)
            {
                return new List<SectionViewModelAdmin>();
            }
            sectionViewModelAdmin = _context.Sections.Where(w => w.ClentId == clientId).Select(x => new SectionViewModelAdmin()
            {
                SectionName = x.Section1,
                SectionId = x.SectionId
            }).ToList();


            if (sectionViewModelAdmin.Count > 0)
            {
                return sectionViewModelAdmin;
            }
            else
            {
                return new List<SectionViewModelAdmin>();
            }
        }
        public async Task<IGeneralResult<string>> ActiveInactiveQuestion(int questionId, int clientId, bool isSuperAdmin, bool isActive)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            Question question = new Question();
            int status = 0;
            if (isSuperAdmin)
            {
                question = _context.Questions.Where(x => x.QuestionId == questionId && x.IsDeleted == false && (clientId == 0 ? true : x.ClentId == clientId)).FirstOrDefault();
                if (question == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Question not found.";
                }
                question.IsActive = isActive;

                _context.Questions.Update(question);
                status = _context.SaveChanges();
                if (status > 0)
                {
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
            if (clientId == 0)
            {
                result.IsSuccess = false;
                result.Message = "ClientId Not Found.";
                return result;
            }

            question = _context.Questions.Where(x => x.QuestionId == questionId && x.IsDeleted == false && (clientId == 0 ? true : x.ClentId == clientId)).FirstOrDefault();
            if (question == null)
            {
                result.IsSuccess = false;
                result.Message = "Question not found.";
                return result;
            }
            question.IsActive = isActive;
            _context.Questions.Update(question);
            status = _context.SaveChanges();
            if (status > 0)
            {
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
        public async Task<IGeneralResult<string>> DeleteQuestion(int questionId, int clientId, bool isSuperAdmin, bool isDelete)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            Question question = new Question();
            int status = 0;
            if (isSuperAdmin)
            {
                question = _context.Questions.Where(x => x.QuestionId == questionId && x.IsDeleted == false && (clientId == 0 ? true : x.ClentId == clientId)).FirstOrDefault();
                if (question == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Question not found.";
                    return result;
                }
                if (question.IsDeleted == true)
                {
                    result.IsSuccess = true;
                    result.Message = "Question  has been already deleted.";
                    return result;
                }

                question.IsDeleted = isDelete;
                _context.Questions.Update(question);
                status = _context.SaveChanges();
                if (status > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Question has been deleted sucessfully.";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Question not deleted. Please try again.";
                }
                return result;
            }
            if (clientId == 0)
            {
                result.IsSuccess = false;
                result.Message = "ClientId Not Found.";
                return result;
            }

            question = _context.Questions.Where(x => x.QuestionId == questionId && x.IsDeleted == false && (clientId == 0 ? true : x.ClentId == clientId)).FirstOrDefault();
            if (question == null)
            {
                result.IsSuccess = false;
                result.Message = "Question not found.";
                return result;
            }
            if (question.IsDeleted == true)
            {
                result.IsSuccess = true;
                result.Message = "Question  has been already deleted.";
                return result;
            }
            question.IsDeleted = isDelete;
            _context.Questions.Update(question);
            status = _context.SaveChanges();
            if (status > 0)
            {
                result.IsSuccess = true;
                result.Message = "Question deleted sucessfully.";
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Question not deleted.";
            }
            return result;
        }
        public async Task<IGeneralResult<QuestionTakeViewModel>> AddOrUpdateQuestion(QuestionTakeViewModel questionTakeView)
        {
            IGeneralResult<QuestionTakeViewModel> res = new GeneralResult<QuestionTakeViewModel>();
            try
            {
                if (questionTakeView.QuestionId > 0)
                {
                    var questiondata = _context.Questions.Where(x => x.QuestionId == questionTakeView.QuestionId).FirstOrDefault();
                    questiondata.Text = questionTakeView.Text;
                    questiondata.QuestionTypeId = questionTakeView.QuestionTypeId;
                    questiondata.GroupId = questionTakeView.GroupId;
                    questiondata.SectionId = questionTakeView.SectionId;
                    questiondata.Marks = questionTakeView.Marks;
                    questiondata.ClentId = questionTakeView.ClentId;
                    var question = _context.Questions.Update(questiondata);
                    questionTakeView.QuestionId = question.Entity.QuestionId;
                    foreach (var option in questionTakeView.QuestionssoptionVm)
                    {
                        var questionoptiondata = _context.QuestionOptions.Where(x => x.OptionId == option.OptionId).FirstOrDefault();
                        questionoptiondata.OptionText = option.OptionText;
                        questionoptiondata.IsCorrect = option.IsCorrect;
                        questionoptiondata.Imagepath = _processUploadFile.GetUploadFile(option.Image);
                        var questionoptions = _context.QuestionOptions.Update(questionoptiondata);
                        _context.SaveChanges();
                        option.OptionId = questionoptions.Entity.OptionId;
                    }
                    res.Data = questionTakeView;
                    res.IsSuccess = true;
                    res.Message = "Question Updated Successfully";
                }
                else
                {
                    Question vm = new Question
                    {
                        Text = questionTakeView.Text,
                        QuestionTypeId = questionTakeView.QuestionTypeId,
                        GroupId = questionTakeView.GroupId,
                        SectionId = questionTakeView.SectionId,
                        Marks = questionTakeView.Marks,
                        ClentId = questionTakeView.ClentId,
                        IsActive = true,
                        IsDeleted = false
                    };
                    var question = _context.Questions.Add(vm);
                    _context.SaveChanges();
                    questionTakeView.QuestionId = question.Entity.QuestionId;
                    foreach (var option in questionTakeView.QuestionssoptionVm)
                    {
                        QuestionOption questionoption = new QuestionOption
                        {
                            QuestionId = questionTakeView.QuestionId,
                            OptionText = option.OptionText,
                            IsCorrect = option.IsCorrect,
                            Imagepath = _processUploadFile.GetUploadFile(option.Image),
                        };
                        var questionoptions = _context.QuestionOptions.Add(questionoption);
                        _context.SaveChanges();
                        option.OptionId = questionoptions.Entity.OptionId;
                        option.Imagepath = baseUrl + questionoption.Imagepath;
                        option.Image = null;
                    }
                    res.Data = questionTakeView;
                    res.IsSuccess = true;
                    res.Message = "Question Added Successfully";
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }
            return res;
        }
    }
}
