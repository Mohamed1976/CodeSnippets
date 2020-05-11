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

namespace ASPNETCoreWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            CultureInfo uiCultureInfo = Thread.CurrentThread.CurrentUICulture;
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            Trace.WriteLine("HomeController.Index()");
            Trace.WriteLine($"uiCultureInfo: {uiCultureInfo.Name}, cultureInfo: {cultureInfo.Name}");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        ////Was autmatically added by VS2019
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
