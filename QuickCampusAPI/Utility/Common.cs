using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Utility
{
    public class Common
    {
        private readonly IConfiguration _configuration;

        //public Common(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}
        //public class GenerateFilePath
        //{
        //    public static string GetCollegeLogoPath(int ID)
        //    {
        //        if (!System.IO.Directory.Exists(Common.Folders.Upload.Admin.CollegeLogo.Physical_Path + "" + ID))
        //        {
        //            System.IO.Directory.CreateDirectory(Common.Folders.Upload.Admin.CollegeLogo.Physical_Path + "" + ID);
        //        }
        //        return Common.Folders.Upload.Admin.CollegeLogo.Physical_Path + "" + ID + "\\";
        //    }
        //}
        //public class Folders : Path
        //{
        //    public static class Upload
        //    {
        //        #region -- Upload Folder Path ---
        //        public static string Physical_Path
        //        {
        //            get
        //            {
        //                return PhysicalPath() + "Upload\\";
        //            }
        //        }
        //        public static string Virtual_Path_Upload
        //        {
        //            get
        //            {
        //                return VirtualPath() + "/Upload";
        //            }
        //        }
        //        #endregion
        //        public static class Admin
        //        {
        //            # region -- Admin Folder Path ---
        //            public static string Physical_Path
        //            {
        //                get
        //                {
        //                    return Common.Folders.Upload.Physical_Path + "Admin\\";
        //                }
        //            }
        //            public static string Virtual_Path_Upload
        //            {
        //                get
        //                {
        //                    return Common.Folders.Upload.Virtual_Path_Upload + "/Admin/";
        //                }
        //            }
        //            #endregion
        //            public static class CollegeLogo
        //            {
        //                # region -- College Logo Folder Path ---
        //                public static string Physical_Path
        //                {
        //                    get
        //                    {
        //                        return Common.Folders.Upload.Admin.Physical_Path + "CollegeLogo\\";
        //                    }
        //                }
        //                public static string Virtual_Path_Upload
        //                {
        //                    get
        //                    {
        //                        return Common.Folders.Upload.Admin.Virtual_Path_Upload + "CollegeLogo/";
        //                    }
        //                }
        //                #endregion
        //            }
        //        }
        //    }

        //}
        ////public class Path
        //{
        //    public static string PhysicalPath()
        //    {
        //        return System.Configuration.ConfigurationManager.AppSettings["PhysicalPath"].ToString();
        //    }
        //    public static string VirtualPath()
        //    {
        //        return System.Configuration.ConfigurationManager.AppSettings["SitePath"].ToString();
        //    }
        //}
    }
}
