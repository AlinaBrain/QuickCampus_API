using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Common.Helper
{
    public class ProcessUploadFile
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private IConfiguration _config;
        private string maxSize;

        public ProcessUploadFile(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
          this.hostingEnvironment = hostingEnvironment;
            _config= configuration;
            maxSize = _config["MaxImageSize"]?? "3000000";
        }

        public IGeneralResult<string> GetUploadFile(IFormFile file)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            try
            {
                string uniqueFileName = "";
                if (file != null)
                {
                    string rootPath = Path.Combine(hostingEnvironment.WebRootPath, "UploadFiles");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName.Replace(" ", String.Empty);
                    string filePath = Path.Combine(rootPath, uniqueFileName);
                    using (var filename = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(filename);
                    }
                    result.IsSuccess = true;
                    result.Message = "File upload successfully";
                    result.Data = uniqueFileName;
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server error " + ex.Message;
            }
            return result;
        }
        public IGeneralResult<string> CheckImage(IFormFile Image)
        {
            IGeneralResult<string> result=new GeneralResult<string>();
            string[] ImageExList = { "jpeg", "jpg", "png" };
            string ImageEx = Image.FileName.Split(".")[1];
            if (!ImageExList.Any(x => x == ImageEx))
            {
                result.Message = "Image should be in jpeg, png or jpg format.";
                return (result);
            }
            if(Image.Length> Convert.ToInt32(maxSize))
            {
                result.Message = "File size must not exceed "+Convert.ToInt32(maxSize)/1000000+"mb";
                return (result);
            }
            result.IsSuccess = true;
            return result;
        }
    }
}
