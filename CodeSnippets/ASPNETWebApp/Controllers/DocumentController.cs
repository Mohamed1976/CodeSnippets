using ASPNETWebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace ASPNETWebApp.Controllers
{
    public class DocumentController : Controller
    {
        public ActionResult Index()
        {
            Trace.WriteLine("ActionResult Index()");
            List <File> files = StorageFolder.GetFiles("~/Documents");
            return View(files);
        }

        //FileResult, Helper method = Controller.File, Returns binary output that is written to the result
        //It has two properties: ContentType and FileDownloadName. FileDownloadName represents the 
        //value that will be defaulted into the Save File dialog box that the browser will open.
        //The FileResult action result supports binary file management, such as retrieving images 
        //from a database and sending them to the client or managing documents on the server.
        public FileResult Download(string id)
        {
            Trace.WriteLine("FileResult Download(string id)");
            int fid = Convert.ToInt32(id);
            List<File> files = StorageFolder.GetFiles("~/Documents");
            File selectedFile = (from file in files
                                 where file.Id == fid 
                                 select file)
                                 .FirstOrDefault();

            string extension = System.IO.Path.GetExtension(selectedFile.FileName);
            string contentType = default;

            if (extension.Equals(".pdf"))
                contentType = "application/pdf";
            if (extension.Equals(".JPG") || extension.Equals(".GIF") || extension.Equals(".PNG"))
                contentType = "application/image";
            Trace.WriteLine("contentType: " + contentType);

            Trace.WriteLine(selectedFile);
            string mime = MimeMapping.GetMimeMapping(selectedFile.FilePath);
            Trace.WriteLine("mime: " + mime);
            //If you dont specify FileName, file opens in WebBrowser
            //https://docs.microsoft.com/en-us/dotnet/api/system.web.mvc.controller.file?redirectedfrom=MSDN&view=aspnet-mvc-5.2
            return File(fileName: selectedFile.FilePath, 
                contentType: mime, 
                fileDownloadName: selectedFile.FileName);

            //return File("~/Files/text.txt", "text/plain");
            //if you want to return the file in byte array then you have to use following code,
            //byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Files/text.txt"));
            //return File(fileBytes, "text/plain");
            //if you don’t want to show the file in the browser and want that your file should be 
            //asked to download then you have to use following overload the file helper method,
            //return File(Url.Content("~/Files/text.txt"), "text/plain", "testFile.txt"); 
        }

        //ContentResult, Helper method Content, Returns a user-defined content type
        //All information specific to ViewResultBase is made available as the view starts to render.
        //ContentResult is a surprisingly flexible action result because it enables you to define the
        //content as a string, the encoding of the content, and the content type.You can return anything
        //from XML (by using a content type of text/xml) to a PDF file(by using application/pdf).
        //Anything that can be streamed as an encoded string and has a well-known content type can
        //be returned as a ContentResult. ContentResult is a way to send encoded and defined 
        //string values to the client. ContentResult (returns content like HTML, Javascript 
        //or any other content. ) Content result is a datatype which is responsible for 
        //the returning of content. You can also give MIME(Multipurpose Internet Mail Extensions) 
        //type in Content helper method to tell the MVC to take appropriate action after 
        //recognizing the content.
        public ContentResult Details(string id)
        {
            Trace.WriteLine("FileResult Details(string id), id: " + id);
            int fid = Convert.ToInt32(id);
            List<File> files = StorageFolder.GetFiles("~/Documents");
            File selectedFile = (from file in files
                                 where file.Id == fid
                                 select file)
                                 .FirstOrDefault();

            string content = "<h1>" + selectedFile.FileName + "</h1><br/><h2>"
                + selectedFile.FilePath + "</h2>";

            return Content(content, MediaTypeNames.Text.Html);
            //return Content("<script> alert('Hi! ASP.NET at work.') </script>");
        }

        /*  But the most interesting thing about ContentResult is that if you do this:
            public string Content()
            {
                return "<h3>Here's a custom content header</h3>";
            }
            MVC actually creates a ContentResult and wraps it around the returned value 
            (and it doesn't have to be a string). Unless you return null, in which case 
            MVC returns an EmptyResult.
        */

        //Three other action results do not start a process returned to the user: 
        //RedirectResult, RedirectToRouteResult, and EmptyResult.The redirect actions 
        //redirect a page or file elsewhere rather than returning it. This is reminiscent 
        //of Response.Redirect from Web Forms that sends a redirect header to the client 
        //browser, which then asks for the new URL.An example of using a redirect action 
        //is this: After a user makes an online purchase, the user is sent to the online 
        //help page for the application. RedirectResult redirects the user to a URL, and 
        //its natural complement, RedirectToRouteResult, sends the user to a named route 
        //in the route table. ActionResults we need for redirecting to other URLs or actions.
        //That works great for redirecting to outside sites from the current app, 
        //but not so much for redirecting to other pages within the same app. 
        //For that, we can use RedirectToRouteResult.
        public RedirectResult RedirectToOtherSite()
        {
            //return Redirect("http://localhost:12060/Home/Index");
            return Redirect("https://edition.cnn.com/");
        }

        public RedirectToRouteResult   RedirectToHomePage()
        {
            //var routeValue = new RouteValueDictionary(new { action = "Index", controller = "Home", area = "" });
            // return RedirectToRoute(routeValue);
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        //EmptyResult returns 200 status code.
        //MVC wants you to use EmptyResult when the action is specifically intended to return nothing.
        //Unlike all of the previous ActionResults though, EmptyResult doesn't have a helper. 
        //Additionally, if an action returns null, MVC will detect that and make it return an EmptyResult.
        public EmptyResult Delete(int? id)
        {
            Trace.WriteLine("public EmptyResult Delete(int? id), id: " + id);
            return new EmptyResult();
            //return null;
        }

        public PartialViewResult EmpList()
        {
            return PartialView();
        }

        public ViewResult ListCars()
        {
            CarService carService = new CarService();
            List<Car> cars = carService.GetAll();
            return View(cars);
        }

        //How to get data from an HTML form to your ASP.NET MVC Core Controller
        //#1 The manual way (using plain old HTML forms)
        [HttpPost]
        public ContentResult RegisterUser(string firstName, string lastName)        
        //public ContentResult RegisterUser([Bind(Include ="FirstName, LastName,")]User user)
        {
            Trace.WriteLine($"[HttpPost] ViewResult RegisterUser(), " +
                $"firstName: {firstName}, lastName: {lastName}");
            return Content($"Hello {firstName} {lastName}");
        }

        //The Request Verification Token check here(the ValidateAntiForgeryToken attribute) 
        //to make sure this form has been posted from our site (and not a malicious site hosted by someone else).
        [HttpGet]
        //[ValidateAntiForgeryToken] TODO
        public ViewResult RegisterUser()
        {
            Trace.WriteLine("[HttpGet] ViewResult RegisterUser()");
            return View();
        }
    }
}