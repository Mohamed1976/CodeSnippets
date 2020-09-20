using System.Collections.Generic;
using System.Linq;
using DataLibrary.BankDemo.Models;
using DataLibrary.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepo _repo;
        private readonly ILogger<BankController> logger;

        public CustomerController(ICustomerRepo repo, ILogger<BankController> logger)
        {
            _repo = repo;
            this.logger = logger;
        }

        /// <summary>
        /// Get all customers.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /api/Customer
        ///     https://localhost:5001/api/Customer
        /// </remarks>
        /// <returns>List of all customers.</returns>
        /// <response code="200">Returns customers.</response>
        /// <response code="500">Returned when there was an error in the repo.</response>
        [HttpGet(Name = "GetAllCustomers")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<Customer>> Get() //=> Ok(_repo.GetAll().ToList());
        {
            IEnumerable<Customer> customers = _repo.GetAll();
            return Ok(customers);
        }

        /// <summary>
        /// Gets a single customer.
        /// </summary>
        /// <param name="id">Primary Key of the customer to retrieve</param>
        /// <remarks>
        /// Sample request:
        ///     GET /api/Customer/1
        ///     https://localhost:5001/api/Customer/14
        /// </remarks>
        /// <returns>Customer</returns>
        /// <response code="200">Returns single customer.</response>
        /// <response code="404">Returned when customer with specific id doesn't exist.</response>
        /// <response code="500">Returned when there was an error in the repo.</response>
        [HttpGet("{id}", Name = "GetCustomer")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Customer> Get([FromRoute] int id)//Get(int id)
        {
            Customer customer = _repo.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        /// <summary>
        /// Gets customers using search string.
        /// </summary>
        /// <param name="searchString">String to search the customers by first and last name.</param>
        /// <remarks>
        /// Sample request:
        ///     https://localhost:5001/api/Customer/Find/san
        /// </remarks>
        /// <returns>Matching customers</returns>
        /// <response code="200">Returns matching customers.</response>
        /// <response code="204">Returned when no content in the response</response>
        /// <response code="500">Returned when there was an error in the repo.</response>
        [HttpGet("Find/{searchString}", Name = "SearchProducts")]
        [Produces("application/json")]
        [ProducesResponseType(200)] //
        [ProducesResponseType(204)] //NoContent()
        [ProducesResponseType(500)]
        public ActionResult<IList<Customer>> Search([FromRoute] string searchString)
        {
            IEnumerable<Customer> customers = _repo.Search(searchString);

            if (customers.Count() == 0)
            {
                return NoContent();
            }

            return Ok(customers);
        }
    }
}
