using ASPNETWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPNETWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //Access Unvalidated data
        //[ValidateInput(false)]
        public ActionResult Create(FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                //Return Get or Post depending on request
                //HttpContext.Request.RequestType
                //Access Unvalidated data  
                //string str1 = HttpContext.Request.Unvalidated.Form["Name"];
                Employee employee = new Employee();
                employee.Name = formCollection["Name"];
                employee.Gender = formCollection["Gender"];
                employee.Age = Convert.ToInt32(formCollection["Age"]);
                employee.Position = formCollection["Position"];
                employee.Office = formCollection["Office"];
                //Check format of date
                //employee.HireDate = Convert.ToDateTime(formCollection["HireDate"]);
                employee.Salary = Convert.ToDecimal(formCollection["Salary"]);
                //_dbContext.Employees.Add(employee);
                //_dbContext.SaveChanges();
                RedirectToAction("Index");
            }
            return View();
        }
    }
}