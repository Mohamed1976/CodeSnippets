using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//Introduced in ASP.NET Core, IStringLocalizer and IStringLocalizer<T> were architected to 
//improve productivity when developing localized apps.IStringLocalizer uses the ResourceManager 
//and ResourceReader to provide culture-specific resources at run time.The simple interface has 
//an indexer and an IEnumerable for returning localized strings.IStringLocalizer doesn't require 
//you to store the default language strings in a resource file. You can develop an app targeted 
//for localization and not need to create resource files early in development. The code below 
//shows how to wrap the string "About Title" for localization.
//https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-3.1
namespace ASPNETCoreWebApp.Controllers
{
    public class InternationalizationController : Controller
    {
        private readonly IStringLocalizer<InternationalizationController> _localizer;

        public InternationalizationController(IStringLocalizer<InternationalizationController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet]
        public ContentResult Index()
        {
            return Content(_localizer["About Title"]);
            //return View();
        }
    }
}
