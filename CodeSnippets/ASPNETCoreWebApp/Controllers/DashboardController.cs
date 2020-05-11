using ASPNETCoreWebApp.Models;
using ASPNETCoreWebApp.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private IWebHostEnvironment _environment;
        private readonly IWebServiceReader webServiceReader;
        private readonly ILogger<DashboardController> logger;

        public DashboardController(IWebHostEnvironment environment, 
            IWebServiceReader webServiceReader, ILogger<DashboardController> logger)
        {
            _environment = environment;
            this.webServiceReader = webServiceReader;
            this.logger = logger;
        }

        //Get
        public IActionResult Index()
        {
            Trace.WriteLine($"public IActionResult Index()");
            return View();
        }

        public IActionResult Documents()
        {
            var path = Path.Combine(_environment.WebRootPath, "Documents");
            Trace.WriteLine($"public IActionResult Documents(), path: {path}");            
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] files = dirInfo.GetFiles();
            foreach (FileInfo fileInfo in files)
            {
                //FullName: C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\ASPNETCoreWebApp\wwwroot\Documents\CreatePdfLogic.pdf, Length: 1638699 Name: CreatePdfLogic.pdf
                Trace.WriteLine($"FullName: {fileInfo.FullName}, " +
                    $"Length: {fileInfo.Length} Name: {fileInfo.Name}");
            }

            return View(files);
        }

        //TODO check best practices
        //<a asp-action="Download" asp-route-fileName="@item.Name">Download</a> 
        //public IActionResult Download(string fileName)
        //<a href="/Dashboard/Download?fileName=ResizeImage.pdf">Download</a> 
        public IActionResult Download(string id)
        {
            Trace.WriteLine($"Download(string id), id: {id}");
            var path = Path.Combine(_environment.WebRootPath, "Documents");
            Trace.WriteLine($"public IActionResult Download(string id), path: {path}");
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            string filepath = (from fileInfo in dirInfo.GetFiles()
                               where fileInfo.Name.Equals(id, StringComparison.Ordinal)
                               select fileInfo.FullName).FirstOrDefault();
            Trace.WriteLine($"filepath: {filepath}");
            FileExtensionContentTypeProvider fileProvider 
                = new FileExtensionContentTypeProvider();

            // Figures out what the content type should be based on the file name.  
            if (!fileProvider.TryGetContentType(filepath, out string contentType))
            {
                Trace.WriteLine($"Unable to find Content Type for file name {filepath}.");
                throw new ArgumentOutOfRangeException($"Unable to find Content Type for file name {filepath}.");
            }

            Trace.WriteLine($"contentType: {contentType}");

            byte[] content = System.IO.File.ReadAllBytes(filepath);
            Trace.WriteLine($"content.Length: {content.Length}");
            string fileName = Path.GetFileName(filepath);

            Trace.WriteLine($"download fileName: {fileName}");

            //var fs = System.IO.File.OpenRead(filepath);
            return File(fileContents: content,
                contentType: contentType,
                fileDownloadName: fileName);
        }

        [HttpPost]
        //Caution with security, see microsoft article 
        //https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-3.1
        //https://github.com/dotnet/AspNetCore.Docs/tree/31ae19f46f6697148397510367347e6d132d4d0a/aspnetcore/mvc/models/file-uploads
        //http://www.dotnet-stuff.com/tutorials/aspnet-core/understanding-file-uploads-in-aspnet-core
        //https://codedocu.com/Details_Mobile?d=2255&a=9&f=372&l=0&v=m&t=Asp-jQuery:-upload-files-with-Ajax
        //https://www.brechtbaekelandt.net/blog/post/uploading-files-with-ajax-and-aspnet-core-2
        //https://blog.elmah.io/upload-and-resize-an-image-with-asp-net-core-and-imagesharp/
        public IActionResult UploadFiles(List<IFormFile> files)
        {
            Trace.WriteLine("IActionResult UploadFiles(List<IFormFile> files)");

            long size = files.Sum(f => f.Length);
            Trace.WriteLine($"file size: {size}");

            string path = Path.Combine(_environment.WebRootPath, "Documents");
            Trace.WriteLine($"public IActionResult Download(string id), path: {path}");

            foreach (IFormFile formFile in files)
            {
                string fullName = Path.Combine(path, formFile.FileName);
                Trace.WriteLine($"File fullName: {fullName}");

                //https://docs.microsoft.com/en-us/dotnet/api/system.io.path.combine?view=netframework-4.8
                //Note when file exists it will be overwritten 
                using (FileStream fileStream =
                    new FileStream(fullName, FileMode.Create, FileAccess.Write))
                {
                    formFile.CopyTo(fileStream);
                }
            }

            return RedirectToAction(nameof(Documents));
        }

        [HttpPost]
        //https://blog.elmah.io/upload-and-resize-an-image-with-asp-net-core-and-imagesharp/
        //Now for the resizing part. Since we no longer have access to the WebImage class, 
        //I'll install an excellent replacement named ImageSharp:
        //ImageSharp is currently in beta but already working out great. 
        //ImageSharp has an Image class that works like WebImage in a lot of ways. 
        //PM> Install-Package SixLabors.ImageSharp -IncludePrerelease
        public IActionResult UploadFilesV2(IFormFile file)
        {
            Trace.WriteLine("DashboardController.UploadFilesV2(IFormFile file)");
            Trace.WriteLine($"FileName: {file.FileName}, ContentType: {file.ContentType}, " +
                $"Length: {file.Length}");

            string path = Path.Combine(_environment.WebRootPath, "Documents");
            Trace.WriteLine($"public IActionResult Download(string id), path: {path}");

            string fullName = Path.Combine(path, file.FileName);
            Trace.WriteLine($"File fullName: {fullName}");

            using var image = Image.Load(file.OpenReadStream());
            image.Mutate(x => x.Resize(256, 256));
            image.Save(fullName);

            //return Ok();
            return RedirectToAction(nameof(Documents));
        }
        
        public IActionResult Nasa()
        {
            ViewBag.Title = "Nasa homepage";
            return View();
        }
    }
}
