using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ASPNETCoreWebApp.Models;
using System.Globalization;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using ASPNETCoreWebApp.Resources;
using System.Reflection;
using Microsoft.IdentityModel.Protocols;
using System.Net;
using Microsoft.Extensions.Primitives;
using ASPNETCoreWebApp.Helpers;
//using Microsoft.AspNetCore.Hosting.Server;

namespace ASPNETCoreWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizerFactory _factory;

        public HomeController(ILogger<HomeController> logger,
            IStringLocalizerFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        /// <summary>
        /// You are developing an ASP.NET MVC application that enables you to edit and save a contact.
        /// The application must not save contacts on an HTTP GET request.
        /// We retrieve the GET and POST methods through this.HttpContext.Request.Method.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="c"></param>
        /// <returns></returns>

        public IActionResult EditContact(int id, Contact c)
        {
            //https://stackoverflow.com/questions/6898598/http-verb-of-current-http-context
            if (String.Equals(HttpContext.Request.Method, "GET", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation($"GET EditContact(int id, Contact c), id = {id} ");
                //c = _contactService.RetrieveContact(id);
                c = new Contact { FirstName = "Donald", LastName="Duck" };
            }
            else if(String.Equals(HttpContext.Request.Method, "POST", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation($"POST EditContact(int id, Contact c), FirstName = {c.FirstName}," +
                    $" LastName = {c.LastName}");
                //_contactService.SaveContact(c);
                return RedirectToAction("Index", "Home");
            }

            return View(c);
        }

        public IActionResult Index()
        {
            //Check attributes of a controller
            RunLogController runLogController = new RunLogController();
            var Attributes = runLogController.GetType().CustomAttributes;
            Trace.WriteLine("Attributes in RunLogController()");
            foreach (var attribute in Attributes)
            {
                Trace.WriteLine($"\tName: {attribute.AttributeType.Name}");
            }

            CultureInfo uiCultureInfo = Thread.CurrentThread.CurrentUICulture;
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            Trace.WriteLine("HomeController.Index()");
            Trace.WriteLine($"uiCultureInfo: {uiCultureInfo.Name}, cultureInfo: {cultureInfo.Name}");
            return View();
        }

        public IActionResult Privacy()
        {
            //https://www.carlrippon.com/httpcontext-in-asp-net-core/
            //You could also use Server.HtmlDecode(
            ViewBag.Message = WebUtility.HtmlDecode(HttpContext.Items["option"] as string);
            return View();
        }

        [HttpGet]
        [Authorize]//(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult MyDocs()
        {
            ViewBag.Title = "MyDocs View@Work you only need to be authenticated.";
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Employee")] //(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Employee")]
        [Authorize(Roles = "Manager")] //(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Manager")]
        public IActionResult MyVideos()
        {
            ViewBag.Title = "MyVideos View@Work only Employees that are Manager allowed.";
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "UserPolicy")]
        public IActionResult MyBooks()
        {
            ViewBag.Title = "MyBooks View@Work only allowed for UserPolicy.";
            return View();
        }

        /// <summary>
        /// The information about the query string is located using one of the following properties: 
        /// HttpContext.Request.QueryString and HttpContext.Request.Query. The difference is that 
        /// the QueryString is basically the raw text string, while the Query property allows you 
        /// to easily access keys and their values. 
        /// 
        /// 
        /// https://asp.mvc-tutorial.com/httpcontext/query-string-get-data/
        /// https://docs.microsoft.com/en-us/dotnet/api/system.web.httprequest.querystring?view=netframework-4.8
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult QueryTest(Contact contact)
        {
            if (String.IsNullOrEmpty(HttpContext.Request.Query["FirstName"]) ||
                String.IsNullOrEmpty(HttpContext.Request.Query["LastName"]))
            {
                RedirectToAction(nameof(HomeController.Index), "Home");
            }

            ViewBag.QueryString = HttpContext.Request.QueryString.Value;
            ViewBag.Contact = $"Contact obj: FirstName = {contact.FirstName}" +
                $", LastName = {contact.LastName}";

            var c = new Contact()
            {
                FirstName = HttpContext.Request.Query["FirstName"],
                LastName = HttpContext.Request.Query["LastName"]
            };

            return View(c);
        }

        [AllowAnonymous]
        [HttpGet]
        //https://docs.microsoft.com/en-us/aspnet/core/performance/caching/response?view=aspnetcore-3.1
        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Clock()
        {
            ViewData["Time"] = DateTime.Now;
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Watch()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;

            ViewData["Time"] = DateTime.Now;
            return View();
        }

        ////Was autmatically added by VS2019
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //FirstChanceException handler is first called
        //Exception Handler Next
        //finally clause is then called, as shown below
        //ASPNETCoreWebApp Error: 0 : Exception Count: 9 exception: Private string Loader() method not implemented yet.
        //ASPNETCoreWebApp Error: 0 : Exception caught: System.NotImplementedException: Private string Loader() method not implemented yet.
        //ASPNETCoreWebApp Error: 0 : View rendering
        public IActionResult Contact()
        {
            string data = null;

            try
            {
                data = Loader();
                return NotFound();
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Exception caught: {ex}");
            }
            finally
            {
                Trace.TraceError($"View rendering");
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private string Loader()
        {
            throw new NotImplementedException("Private string Loader() method not implemented yet.");
        }

        public IActionResult GetProducts()
        {
            //Return the http method Get Post
            //HttpContext.Request.Method

            //https://stackoverflow.com/questions/41289737/get-the-current-culture-in-a-controller-asp-net-core
            CultureInfo uiCultureInfo = Thread.CurrentThread.CurrentUICulture;
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            ViewData["CurrentCulture"] = "uiCulture: " + uiCultureInfo +
                ", culture: " + cultureInfo;

            //Should be service as shown below
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-3.1
            //https://damienbod.com/2017/11/01/shared-localization-in-asp-net-core-mvc/

            var type = typeof(MyDictionary);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            IStringLocalizer _localizer = 
                _factory.Create(nameof(MyDictionary), assemblyName.Name);

            ViewBag.Title = _localizer["Title"];

            return View();
        }

        //You are developing an ASP.NET MVC web application that enables users to open Microsoft Excel files.
        //You need to enable users to open Excel files.
        //https://www.codeproject.com/Questions/481262/contentplustypeplusforplusxlsxplusfile
        //The current implementation of the ExcelResult class can be found in ExcelResult.
        //The View() is not used
        //
        public IActionResult GetExcel()
        {
            return new ExcelResult();
        }

        public ActionResult Chat()
        {
            return View();
        }

        //SignalR examples    
        //https://gigi.nullneuron.net/gigilabs/getting-started-with-signalr/
        //https://stackoverflow.com/questions/42313284/calling-signalr-hub-method-from-code-behind
        //https://docs.microsoft.com/en-us/aspnet/signalr/overview/getting-started/tutorial-getting-started-with-signalr
        //https://docs.microsoft.com/en-us/aspnet/core/signalr/hubs?view=aspnetcore-3.1
        public ActionResult Forum()
        {
            return View();
        }
    }
}

/*
    public class LocService
    {
        private readonly IStringLocalizer _localizer;
 
        public LocService(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("SharedResource", assemblyName.Name);
        }
 
        public LocalizedString GetLocalizedHtmlString(string key)
        {
            return _localizer[key];
        }
    }

*/
