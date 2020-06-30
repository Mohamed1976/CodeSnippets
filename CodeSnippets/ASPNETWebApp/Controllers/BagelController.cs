using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPNETWebApp.Controllers
{
    public class BagelController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBagel(string bagelName)
        {
            ViewBag.Name = bagelName;
            return View();
        }
    }
}