using ASPNETCoreWebApp.Data;
using ASPNETCoreWebApp.Data.Entities;
using ASPNETCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PatientsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;

        public PatientsController(ILogger<ProductsController> logger)
        {
            this._logger = logger;
        }

        [Route("GetDealPrice/{productId}")]
        [HttpGet]
        public IActionResult GetDealPrice(string productId)
        {
            string currencySymbol = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
            Debug.WriteLine($"GetDealPrice(string productId), productId: {productId}");
            return new JsonResult(currencySymbol + ", Hello: " + productId);
        }

        // GET: api/Get
        [HttpGet]
        public ActionResult<IEnumerable<Patient>> Get()
        {
            List<Patient> patients = new List<Patient>
            {
                new Patient
                {
                    RegNo = "2017-0001",
                    Name = "Nishan",
                    Address = "Kathmandu",
                    PhoneNo = "9849845061",
                    AdmissionDate = DateTime.Now
                },
                new Patient
                {
                    RegNo = "2017-0002",
                    Name = "Namrata Rai",
                    Address = "Bhaktapur",
                    PhoneNo = "9849845062",
                    AdmissionDate = DateTime.Now
                },
           };

            try
            {
                Trace.WriteLine($"ProductsController.GetAllPatients()");
                return Ok(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get All Patients: {ex}");
                return BadRequest("Failed to get All Patients");
            }
        }
    }
}
