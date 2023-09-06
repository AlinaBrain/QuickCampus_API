using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Collections;
using System.Linq.Expressions;

namespace QuickCampus_Core.Services
{
    public class QuestionService : BaseRepository<QuikCampusDevContext, Question>,IQuestion
    {
        private readonly IConfiguration _config;
        private readonly QuikCampusDevContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly string basepath;
        private string baseUrl;
        public QuestionService(QuikCampusDevContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config)
        {
            _config = config;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            baseUrl = _config.GetSection("APISitePath").Value;
        }
        public async Task<IGeneralResult<List<QuestionViewModelAdmin>>> GetAllQuestion(int clientid, bool issuperadmin, int pageStart, int pageSize)
        {
            IGeneralResult<List<QuestionViewModelAdmin>> result = new GeneralResult<List<QuestionViewModelAdmin>>();
            List<QuestionViewModelAdmin> record = new List<QuestionViewModelAdmin>();
            if (issuperadmin)
            {
                var totalCount = result.Data = _context.Questions.Where(x => x.IsDeleted == false && (clientid == 0 ? true : x.ClentId == clientid)).Select(x => new QuestionViewModelAdmin()
                {
                    QuestionId = x.QuestionId,
                    QuestionTypeName = x.QuestionType.QuestionType1,
                    QuestionSection = x.Section.Section1,
                    QuestionGroup = x.Group.GroupName,
                    Question = x.Text,
                    IsActive = x.IsActive ?? false
                }).ToList();
                result.Data = _context.Questions.Where(x => x.IsDeleted == false && (clientid == 0 ? true : x.ClentId == clientid)).Select(x => new QuestionViewModelAdmin()
                {
                    QuestionId = x.QuestionId,
                    QuestionTypeName = x.QuestionType.QuestionType1,
                    QuestionSection = x.Section.Section1,
                    QuestionGroup = x.Group.GroupName,
                    Question = x.Text,
                    IsActive = x.IsActive ?? false
                }).OrderByDescending(x => x.QuestionId).Skip(pageStart).Take(pageSize).ToList();
                if (result.Data.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Record Fetch Successfully";
                    result.TotalRecordCount = totalCount.Count();
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
                Question = x.Text,
                IsActive = x.IsActive ?? false
            }).ToList();
            result.Data = _context.Questions.Where(x => x.IsDeleted == false && x.ClentId == clientid).Select(x => new QuestionViewModelAdmin()
            {
                QuestionId = x.QuestionId,
                QuestionTypeName = x.QuestionType.QuestionType1,
                QuestionSection = x.Section.Section1,
                QuestionGroup = x.Group.GroupName,
                Question = x.Text,
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

                }).FirstOrDefault();

                if (question == null)
                {
                    res.IsSuccess = false;
                    res.Message = "Question not Succefully";
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

        public async Task<IGeneralResult<string>> AddQuestion( QuestionViewModelAdmin model, bool isSuperAdmin)
        {
            List<QuestionType> allQuestions = new List<QuestionType>();
            List<Section> allSections = new List<Section>();
            List<Groupdl> allGroups = new List<Groupdl>();
            IGeneralResult<string> res = new GeneralResult<string>();
            if (isSuperAdmin)
            {
                allQuestions = _context.QuestionTypes.Where(x => (model.ClientId == 0 ? true : x.ClentId == model.ClientId)).ToList();
                allSections = _context.Sections.Where(x => (model.ClientId == 0 ? true : x.ClentId == model.ClientId)).ToList();
                allGroups = _context.Groupdls.Where(x => (model.ClientId == 0 ? true : x.ClentId == model.ClientId)).ToList();
            }
            else
            {
                allQuestions = _context.QuestionTypes.Where(x => x.ClentId == model.ClientId).ToList();
                allSections = _context.Sections.Where(x => x.ClentId == model.ClientId).ToList();
                allGroups = _context.Groupdls.Where(x => x.ClentId == model.ClientId).ToList();

                if (model.ClientId == 0)
                {
                    res.IsSuccess = false;
                    res.Message = "invalid client Id";
                    return res;
                }
            }

            bool isExistQuestionType = allQuestions.Any(a => a.QuestionTypeId == model.QuestionTypeId);
            bool isExistGroup = allGroups.Any(a => a.GroupId == model.GroupId);
            bool isExistSection = allSections.Any(a => a.SectionId == model.SectionId);

            if (!isExistQuestionType)
            {
                res.IsSuccess = false;
                res.Message = "invalid questiontype";
                return res;
            }
            else if (!isExistGroup)
            {
                res.IsSuccess = false;
                res.Message = "invalid group";
                return res;
            }
            else if (!isExistSection)
            {
                res.IsSuccess = false;
                res.Message = "invalid section";
                return res;
            }
            bool isExist = _context.Questions.Any(w => w.IsDeleted != true && w.QuestionTypeId == model.QuestionTypeId && w.SectionId == model.SectionId && w.GroupId == model.GroupId);

            if (isExist)
            {
                res.IsSuccess = false;
                res.Message = "Question Already Exist";
                return res;
            }

            int? marks = (int)_context.QuestionTypes.Where(y => y.QuestionTypeId == model.QuestionTypeId).SingleOrDefault().Marks;
            Question question = new Question()
            {
                QuestionTypeId = model.QuestionTypeId,
                SectionId = model.SectionId,
                GroupId = model.GroupId,
                Text = model.Question,
                Marks = marks,
                IsActive = true,
                IsDeleted = false,
                ClentId=model.ClientId
            };
            var result = _context.Questions.Add(question);
            foreach (var item in model.options)
            {
                var fileName = string.Empty;
                byte[] file = null;

                QuestionOption questionoption = new QuestionOption()
                {
                    QuestionId = question.QuestionId,
                    OptionText = item.OptionText,
                    IsCorrect = item.IsCorrect,
                    OptionImage = item.Imagepath != null ? ProcessUploadFile(item) : item.OptionImage,
                    
                    Image = file
                };
                question.QuestionOptions.Add(questionoption);
            }
            int status = _context.SaveChanges();
            if (status > 0)
            {
                res.IsSuccess = true;
                res.Message = "Question has been added successfully.";
            }
            else
            {
                res.IsSuccess = false;
                res.Message = "Question has not been created.";
                res.Data = null;
            }
            return res;
        }

        public async Task<IGeneralResult<string>> UpdateQuestion(QuestionViewModelAdmin model, bool isSuperAdmin)
        {
            IGeneralResult<string> res = new GeneralResult<string>();
            Question question = new Question();


            List<QuestionType> allQuestions = new List<QuestionType>();
            List<Section> allSections = new List<Section>();
            List<Groupdl> allGroups = new List<Groupdl>();

            if (isSuperAdmin)
            {
                allQuestions = _context.QuestionTypes.Where(x => (model.ClientId == 0 ? true : x.ClentId == model.ClientId)).ToList();
                allSections = _context.Sections.Where(x => (model.ClientId == 0 ? true : x.ClentId == model.ClientId)).ToList();
                allGroups = _context.Groupdls.Where(x => (model.ClientId == 0 ? true : x.ClentId == model.ClientId)).ToList();
                question = _context.Questions.Include(x => x.QuestionOptions).FirstOrDefault(x => x.QuestionId == model.QuestionId && x.IsDeleted == false && (model.ClientId == 0 ? true : x.ClentId == model.ClientId));

            }
            else
            {

                allQuestions = _context.QuestionTypes.Where(x => x.ClentId == model.ClientId).ToList();
                allSections = _context.Sections.Where(x => x.ClentId == model.ClientId).ToList();
                allGroups = _context.Groupdls.Where(x => x.ClentId == model.ClientId).ToList();
                if (model.ClientId == 0)
                {
                    res.Message = "Invalid client ";
                    res.IsSuccess = false;
                    return res;
                }

                question = _context.Questions.Include(x => x.QuestionOptions).FirstOrDefault(x => x.QuestionId == model.QuestionId && x.ClentId == model.ClientId && x.IsDeleted == false);

            }

            bool isExistQuestionType = allQuestions.Any(a => a.QuestionTypeId == model.QuestionTypeId);
            bool isExistGroup = allGroups.Any(a => a.GroupId == model.GroupId);
            bool isExistSection = allSections.Any(a => a.SectionId == model.SectionId);

            if (!isExistQuestionType)
            {
                res.IsSuccess = false;
                res.Message = "invalid questiontype";
                return res;
            }
            else if (!isExistGroup)
            {
                res.IsSuccess = false;
                res.Message = "invalid group";
                return res;
            }
            else if (!isExistSection)
            {
                res.IsSuccess = false;
                res.Message = "invalid section";
                return res;
            }


            if (question == null)
            {
                res.IsSuccess = false;
                res.Message = "Question not found";
                return res;
            }

            bool isExist = _context.Questions.Any(w => w.IsDeleted != true && w.QuestionTypeId == model.QuestionTypeId && w.SectionId == model.SectionId && w.GroupId == model.GroupId && w.QuestionId != model.QuestionId);

            if (isExist)
            {
                res.IsSuccess = false;
                res.Message = "Question Already Exist";
                return res;
            }

            question.QuestionTypeId = model.QuestionTypeId;
            question.Text = model.Question;
            question.GroupId = model.GroupId;
            question.SectionId = model.SectionId;
            question.Marks = _context.QuestionTypes.Where(y => y.QuestionTypeId == model.QuestionTypeId).SingleOrDefault().Marks;
            ArrayList itemIdToBeNotRemoved = new ArrayList();
            foreach (var item in model.options)
            {
                itemIdToBeNotRemoved.Add(item.OptionId);

                if (item.OptionId > 0)
                {
                    var options = question.QuestionOptions.SingleOrDefault(y => y.OptionId == item.OptionId);
                    options.OptionText = item.OptionText;
                    options.IsCorrect = item.IsCorrect;

                }
                else
                {
                    QuestionOption questionoption = new QuestionOption()
                    {
                        QuestionId = question.QuestionId,
                        OptionText = item.OptionText,
                        IsCorrect = item.IsCorrect,

                    };
                    question.QuestionOptions.Add(questionoption);

                }


            }
            foreach (var op in question.QuestionOptions.ToList())
            {
                if (!itemIdToBeNotRemoved.Contains(op.OptionId)) { question.QuestionOptions.Remove(op); }
            }
            int result = _context.SaveChanges();
            if (result > 0)
            {
                res.IsSuccess = true;
                res.Message = "Question has been updated successfully.";
            }
            else
            {
                res.IsSuccess = false;
                res.Message = "something went worng.";
            }
            return res;
        }

        private string ProcessUploadFile([FromForm] OptionViewModelAdmin model)
        {
            List<string> url = new List<string>();
            string uniqueFileName = null;
            if (model.Imagepath != null)
            {
                string photoUoload = Path.Combine(_hostingEnvironment.WebRootPath, "UploadFiles");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Imagepath.FileName;
                string filepath = Path.Combine(photoUoload, uniqueFileName);
                using (var filename = new FileStream(filepath, FileMode.Create))
                {
                    model.Imagepath.CopyTo(filename);
                }
            }

            url.Add(Path.Combine(baseUrl, uniqueFileName));
            return url.FirstOrDefault();
        }
    }
}
