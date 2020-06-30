using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPNETWebApp.Controllers
{
    public class ArticleController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Show(string productName)
        {
            ViewBag.Title = productName;
            return View();
        }
    }
}