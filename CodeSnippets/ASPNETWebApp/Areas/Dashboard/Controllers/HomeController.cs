using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPNETWebApp.Areas.Dashboard.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Dashboard/Home/
        public ActionResult Index(int? id)
        {
            Trace.WriteLine(string.Format("Areas.Dashboard.Controllers.HomeController" +
                ", public ActionResult Index(int? id)"));
            Trace.WriteLine(string.Format("id: {0}", id));
            return View();
        }
    }
}