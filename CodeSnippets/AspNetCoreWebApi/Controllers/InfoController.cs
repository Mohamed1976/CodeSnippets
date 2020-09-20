using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWebApi.Controllers
{
    public class InfoController : Controller
    {
        private readonly IEnumerable<Product> products = new List<Product>()
        {
            new Product(){ ID = 1, Name = "Mobile" },
            new Product(){ ID = 2, Name = "Laptop" },
            new Product(){ ID = 3, Name = "IPad" }
        };

        //The concept of OutputCache hasn't made it into the Core version of the .NET framework
        //https://asp.mvc-tutorial.com/caching/outputcache/
        //https://www.nuget.org/packages/WebEssentials.AspNetCore.OutputCaching
        //[OutputCache(Duration = 60)]
        public IActionResult Index()
        {
            ViewData["Message"] = "Example using outputcache.";
            ViewData["SelectedTime"] = DateTime.Now;

            return View(products);
        }
    }

    public class Product
    {
        public int ID { set; get; }
        public string Name { set; get; }
    }
}
