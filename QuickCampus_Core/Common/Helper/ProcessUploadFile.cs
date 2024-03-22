using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

        public ProcessUploadFile(IHostingEnvironment hostingEnvironment)
        {
          this.hostingEnvironment = hostingEnvironment;
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
    }
}
