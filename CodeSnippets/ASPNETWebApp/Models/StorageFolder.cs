using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ASPNETWebApp.Models
{
    public static class StorageFolder
    {
        public static List<File> GetFiles(string path)
        {
            //https://docs.microsoft.com/en-us/dotnet/api/system.web.httpserverutility.mappath?view=netframework-4.8
            string filePath = HttpContext.Current.Server.MapPath(path);
            Trace.WriteLine(@"Server.MapPath: " + filePath);
            DirectoryInfo dirInfo = new DirectoryInfo(filePath);

            int index = 0;
            List<File> files = new List<File>();
            foreach (FileInfo fileInfo in dirInfo.GetFiles())
            {
                File file = new File(index++, fileInfo.Name, fileInfo.FullName);
                files.Add(file);
                Trace.WriteLine(file);
            }

            return files;
        }



    }
}