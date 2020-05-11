using ASPNETCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

//At the lowest level, you can get IStringLocalizerFactory out of Dependency Injection:
//https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-3.1
//The code below demonstrates each of the two factory create methods.
//You can partition your localized strings by controller, area, or have 
//just one container.In the sample app, a dummy class named SharedResource 
//is used for shared resources.

namespace ASPNETCoreWebApp.Controllers
{
    public class StringLocalizerController : Controller
    {
        private readonly IStringLocalizer _localizer;
        private readonly IStringLocalizer _localizer2;

        public StringLocalizerController(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create(type);
            _localizer2 = factory.Create(nameof(SharedResource), assemblyName.Name);
        }

        public IActionResult Index()
        {
            ViewData["Message"] = _localizer["Your application description page."] 
                + " loc 2: " + _localizer2["Your application description page."];
            return View();
        }
    }
}
