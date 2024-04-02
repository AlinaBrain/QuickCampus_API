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
    public class QuestionService : BaseRepository<BtprojecQuickcampustestContext, Question>, IQuestion
    {
        private readonly IConfiguration _config;
        private readonly BtprojecQuickcampustestContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly string basepath;
        private string baseUrl;
        private readonly ProcessUploadFile _uploadFile;

        public QuestionService(BtprojecQuickcampustestContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ProcessUploadFile processUploadFile)
        {
            _config = config;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            baseUrl = _config.GetSection("APISitePath").Value;
            _uploadFile = processUploadFile;
        }
       
    }
}
