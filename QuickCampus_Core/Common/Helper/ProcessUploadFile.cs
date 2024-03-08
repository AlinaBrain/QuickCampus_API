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
        public string GetUploadFile(IFormFile file)
        {

            string uniqueFileName = null;
            if (file != null)
            {
                string photoUpload = Path.Combine(hostingEnvironment.WebRootPath, "UploadFiles");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName.Replace(" ", String.Empty);
                string filepath = Path.Combine(photoUpload, uniqueFileName);
                using (var filename = new FileStream(filepath, FileMode.Create))
                {
                    file.CopyTo(filename);
                }
            }
            //string basepath = baseUrl + uniqueFileName;
            return uniqueFileName;
        }
    }
}
