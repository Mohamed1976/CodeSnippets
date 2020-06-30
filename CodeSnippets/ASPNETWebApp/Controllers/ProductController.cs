using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPNETWebApp.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : Controller
    {

        [Route]
        public ActionResult Index()
        {
            return View();
        }

        [Route("{id}")]
        public ActionResult GetProducts(int id)
        {
            Debug.WriteLine($"Entering public ActionResult GetProducts(int id), id: {id}");
            //var model = new { id = id, name = "Shoe", brand = "Adidas" };
            //ViewBag.Product = model;
            ViewBag.id = id;
            ViewBag.name = "Shoe";
            ViewBag.brand = "Adidas";

            return View();
        }
    }
}