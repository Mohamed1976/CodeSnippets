using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETWebApp.Models
{
    public class File
    {
        public File(int id, string fileName, string filePath)
        {
            Id = id;
            FileName = fileName;
            FilePath = filePath;
        }

        public int Id { get; private set; }
        public string FileName { get; private set; }
        public string FilePath { get; private set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, FileName: {1}, FilePath: {2}", Id, FileName, FilePath);
        }
    }
}