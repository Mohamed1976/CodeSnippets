using ASPNETCoreWebApp.Helpers;
using ASPNETCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Controllers
{
    public class BookingController : Controller
    {

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([ModelBinder(BinderType = typeof(ReservationModelBinder))] ReservationLocation reservationLocation)
        {
            if(ModelState.IsValid)
            {
                //RedirectToAction()
            }

            return View();
        }
    }
}
