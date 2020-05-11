using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPNETWebApp.Controllers
{
    public class BlogController : Controller
    {
        //public ActionResult Index(int? id) //id optional
        //{
        //    return View();
        //}

        //"Default" MapRoute 
        //public ActionResult Index()
        //{
        //    Trace.WriteLine(string.Format("BlogController, ActionResult Index()"));
        //    return View();
        //}

        ////"Default" MapRoute
        //public ActionResult Index(string id)
        //{
        //    Trace.WriteLine(string.Format("BlogController, ActionResult Index(string id)"));
        //    Trace.WriteLine(string.Format("id: {0}", id));
        //    return View();
        //}
        //return Content("<b>Car name Is " + name + "</b>");

        //https://localhost:44396/Blog/aspnet-mvc-routing-examples/JQ   (URL examples)
        public ActionResult ListDetails(string title, string id)
        {
            Trace.WriteLine(string.Format("BlogController, public ActionResult " +
                "ListDetails(string publication, string isbn)"));
            Trace.WriteLine(string.Format("title: {0}, id: {1}", title, id));
            return View("Index");
        }

        //https://localhost:44396/Blog/2020/04/10/Computable   (URL examples)
        public ActionResult List(int year, int month, int day, string title)
        {
            Trace.WriteLine(string.Format("BlogController, public ActionResult List(int year, " +
                "int month, int day, string title)"));
            Trace.WriteLine(string.Format("year: {0}, month: {1}, day: {2}, title: {3}", year, month, day, title));
            return View("Index");
        }


        //GET Blog/Index
        //https://localhost:44396/Blog/aspnet-mvc-routing-examples-JQ   (URL examples)
        //Because this is the get action of the Index page, we can also specify the URL as:  
        //https://localhost:44396/Blog/?title=aspnet-mvc-routing-examples&id=JQ
        public ActionResult Index(string title, string id)
        {
            Trace.WriteLine(string.Format("BlogController, public ActionResult " +
                "Index(string title, string id)"));
            Trace.WriteLine(string.Format("title: {0}, id: {1}", title, id));
            return View();
        }
    }
}