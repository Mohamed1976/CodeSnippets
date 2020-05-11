using ASPNETCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Controllers
{
    public class UserController : Controller
    {
        //Get
        public ViewResult Index()
        {
            Trace.WriteLine("UserController, public ViewResult Index()");
            return View(new CarService().GetAll());
        }

        //GET
        [HttpGet]
        public IActionResult Create()
        {
            Trace.WriteLine("UserController, public IActionResult Create()");
            return View();
        }

        //POST
        [HttpPost]
        //[Bind("FirstName, LastName")]User user, Salary of users is not included
        //You can issue other mapping helpers as part of the Bind attribute, such as 
        //Include and Exclude.The Include and Exclude helpers give you additional 
        //flexibility when working with binding because they give you control over 
        //what items should be bound.Consider a situation in which you are working 
        //on a human resources application.One page of the application is bound to 
        //an Employee object and has all the information about them, including home 
        //address, phone number, and salary. You want your employees to be able to 
        //modify the address and phone number, but not the salary. If the page uses 
        //weak binding to the action, a knowing user could insert an input field with 
        //a salary value, and the model binder would map it, which could result in 
        //unauthorized changes to the data.However, by explicitly listing it as an 
        //Exclude, the model binder skips that field, regardless of it being in 
        //the forms collection:        
        public IActionResult Create([Bind("FirstName, LastName")]User user)
        {
            Trace.WriteLine("UserController, public IActionResult Create(string firstName)");
            Trace.WriteLine($"FirstName: {user.FirstName}, LastName: {user.LastName}, " +
                $"Salary: {user.Salary}, ModelState.IsValid: {ModelState.IsValid}");

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), nameof(User));
            }

            return View(user);
        }
    }
}
