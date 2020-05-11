using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Controllers
{
    public class BookController : Controller
    {
        private readonly IHtmlLocalizer<BookController> _localizer;

        public BookController(IHtmlLocalizer<BookController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Index(string name)
        {
            ViewData["MyMessage"] = _localizer["Your contact page."];
            ViewData["Message"] = _localizer["<b>Hello</b><i> {0}</i>", name];
            return View();
        }
    }
}
