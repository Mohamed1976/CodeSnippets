using ASPNETCoreWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

// When using Ajax install client library     
//<script src="~/lib/jquery-ajax-unobtrusive/jquery.unobtrusive-ajax.min.js"></script>
//https://www.jerriepelser.com/blog/deleting-records-aspnet-core-ajax/
//https://github.com/aspnet/jquery-ajax-unobtrusive
namespace ASPNETCoreWebApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ILogger<CustomersController> logger;
        private readonly ICustomersRepository customersRepository;

        public CustomersController(ILogger<CustomersController> logger, ICustomersRepository customersRepository)
        {
            this.logger = logger;
            this.customersRepository = customersRepository;
        }

        public IActionResult Index()
        {
            return View(customersRepository.Customers);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Trace.WriteLine($"CustomersController.Delete(int id), id: {id}");
            customersRepository.Delete(id);
            //When using AJAX
            //return RedirectToAction(nameof(Index));
            return NoContent();
        }
    }
}
