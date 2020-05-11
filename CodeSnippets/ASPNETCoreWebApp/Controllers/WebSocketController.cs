using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Controllers
{
    public class WebSocketController : Controller
    {
        private readonly ILogger<WebSocketController> logger;

        public WebSocketController(ILogger<WebSocketController> logger)
        {
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ViewResult ServerTime()
        {
            ViewBag.Message = "This page displays the Server Time.";
            return View();
        }

        public PartialViewResult Time()
        {
            ViewBag.ServerTime = DateTime.Now.ToString();

            return PartialView("_TimePartial");
        }
    }
}
