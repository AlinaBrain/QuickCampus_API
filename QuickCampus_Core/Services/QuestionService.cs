using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QuickCampus_Core.Services
{
    public class QuestionService : IQuestion
    {
        private readonly QuikCampusDevContext _context;
        public QuestionService(QuikCampusDevContext context)
        {
            _context = context;
        }
        public async  Task<List<QuestionViewModelAdmin>> GetAllQuestion()
        {
            List<QuestionViewModelAdmin> record = new List<QuestionViewModelAdmin>();
               record =  _context.Questions.Where(x => x.IsDeleted == false && x.IsActive == true).Select(x => new QuestionViewModelAdmin()
                {
                    QuestionId = x.QuestionId,
                    QuestionTypeName = x.QuestionType.QuestionType1,
                    QuestionSection = x.Section.Section1,
                    QuestionGroup = x.Group.GroupName,
                    Question = x.Text,
                    IsActive = x.IsActive ?? false
                }).ToList();
            return record;

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
        public  async Task<List<GroupViewModelAdmin>> GetAllGroups()
        {
            var Groups = await _context.Groups.Select(x => new GroupViewModelAdmin()
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

      
    }
}