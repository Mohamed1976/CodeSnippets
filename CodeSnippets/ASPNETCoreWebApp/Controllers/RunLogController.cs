using ASPNETCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ASPNETCoreWebApp.Filters;

/*
Security:
You have the following security requirements:
The application is configured to use forms authentication.
Users must be logged on to insert runner data.
Users must be members of the Admin role to edit or delete runner data.
There are no security requirements for viewing runner data.
You need to protect the application against cross-site request forgery.
Passwords are hashed by using the SHA1 algorithm.


*/
namespace ASPNETCoreWebApp.Controllers
{
    public class RunLogController : Controller
    {
        private List<LogModel> logModels = new List<LogModel>()
        {
            new LogModel() { Id = 1, Distance=2.34, RunDate=DateTime.Now.AddHours(-7), Time=new TimeSpan(1,12,30)},
            new LogModel() { Id = 2, Distance=3.45, RunDate=DateTime.Now.AddHours(-6), Time=new TimeSpan(1,54,30)},
            new LogModel() { Id = 3, Distance=4.55, RunDate=DateTime.Now.AddHours(-5), Time=new TimeSpan(1,55,30)},
            new LogModel() { Id = 4, Distance=5.32, RunDate=DateTime.Now.AddHours(-8), Time=new TimeSpan(1,32,30)},
            new LogModel() { Id = 5, Distance=6.13, RunDate=DateTime.Now.AddHours(-9), Time=new TimeSpan(1,22,30)},
        };

        [RequireHttps]
        [CustomExceptionFilter]
        public IActionResult GetLog()
        {            
            //throw new Exception("This is some exception!!!");
            //Should be login view
            Login("Mohamed", "Welcome Moo");
            return View(logModels);
        }

        public IActionResult InsertLog()
        {
            return View();
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        public IActionResult InsertLog(LogModel log)
        {
            Debug.WriteLine($"InsertLog, ModelState.IsValid: {ModelState.IsValid}");
            Debug.WriteLine($"Id: {log.Id}, Distance: {log.Distance}, Time: {log.Time}");

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(GetLog));
            }
            else
            {
                return View(log);
            }
        }

        public IActionResult DeleteLog(int id)
        {
            System.Diagnostics.Contracts.Contract.Requires<ArgumentException>(id > 0);
            //System.Diagnostics.Contracts.Contract.Assume(id > 0);

            return RedirectToAction(nameof(GetLog));
        }

        public IActionResult EditLog(int id)
        {
            return View(logModels.Where(x => x.Id == id).FirstOrDefault());
        }

        [HttpPost]
        [ActionName("EditLog")]
        [ValidateAntiForgeryToken]
        public IActionResult EditLogValidated(LogModel log)
        {
            Debug.WriteLine($"EditLogValidated, ModelState.IsValid: {ModelState.IsValid}");
            Debug.WriteLine($"Id: {log.Id}, Distance: {log.Distance}, Time: {log.Time}");
            
            if(ModelState.IsValid)
            {
                return RedirectToAction(nameof(GetLog));
            }
            else
            {
                return View(log);
            } 
        }

        public void Login(string username, string password)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(username + password);
            byte[] hash = SHA256.Create().ComputeHash(buffer);
            //bool isValid = ComparePassword(username,hash);
            Debug.WriteLine($"username: {username}, password: {password}");
            Debug.WriteLine($"hash: {BitConverter.ToString(hash).Replace("-", "")}");
        }
    }
}
