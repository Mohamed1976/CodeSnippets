using AspNetCoreWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApi.Controllers
{
    // base address: api/customers
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private ICustomerRepository repo;
        private readonly ILogger<CustomersController> logger;

        // constructor injects repository registered in Startup
        public CustomersController(ICustomerRepository repo,
            ILogger<CustomersController> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        // GET: api/customers
        // GET: api/customers/?country=[country]
        // this will always return a list of customers even if its empty
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DataLibrary.BankDemo.Models.Customer>))]
        public async Task<IEnumerable<DataLibrary.BankDemo.Models.Customer>> GetCustomers()
        {
            logger.LogInformation("CustomersController, GetCustomers()");
            return await repo.RetrieveAllAsync();
        }

        // GET: api/customers/[id]
        [HttpGet("{id}", Name = nameof(GetBankCustomer))] // named route
        [ProducesResponseType(200, Type = typeof(DataLibrary.BankDemo.Models.Customer))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBankCustomer(int id)
        {
            logger.LogInformation($"public async Task<IActionResult> GetCustomer(int id), id : {id}");
            var customer = await repo.RetrieveAsync(id);
            if (customer == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(customer); // 200 OK with customer in body
        }

        // POST: api/customers
        // BODY: Customer (JSON, XML)
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(DataLibrary.BankDemo.Models.Customer))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> 
            Create([FromBody] DataLibrary.BankDemo.Models.Customer customer)
        {
            logger.LogInformation($"async Task<IActionResult> Create([FromBody] DataLibrary.BankDemo.Models.Customer " +
                $"customer), FirstName: {customer.FirstName}, LastName: {customer.LastName}");
            if (customer == null)
            {
                return BadRequest(); // 400 Bad request
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 Bad request
            }
            else
            {
                DataLibrary.BankDemo.Models.Customer added = await repo.CreateAsync(customer);

                return CreatedAtRoute( // 201 Created 
                    routeName: nameof(GetBankCustomer),
                    routeValues: new { id = added.Id },
                    value: added);
            }
        }

        // PUT: api/customers/[id]
        // BODY: Customer (JSON, XML)
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, 
            [FromBody] DataLibrary.BankDemo.Models.Customer customer)
        {
            logger.LogInformation($"Task<IActionResult> Update(int id, [FromBody] DataLibrary.BankDemo.Models.Customer " +
                $"customer, id: {id}, customer.Id: {customer.Id}, FirstName: {customer.FirstName}");
            if (customer == null || customer.Id != id)
            {
                return BadRequest(); // 400 Bad request
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 Bad request
            }

            DataLibrary.BankDemo.Models.Customer existing =
                await repo.RetrieveAsync(id);

            if (existing == null)
            {
                return NotFound(); // 404 Resource not found
            }

            await repo.UpdateAsync(id, customer);
            return new NoContentResult(); // 204 No content
        }

        // DELETE: api/customers/[id]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            logger.LogInformation("public async Task<IActionResult> Delete(string id)");
            var existing = await repo.RetrieveAsync(id);
            if (existing == null)
            {
                return NotFound(); // 404 Resource not found
            }

            bool? deleted = await repo.DeleteAsync(id);

            if (deleted.HasValue && deleted.Value) // short circuit AND
            {
                return new NoContentResult(); // 204 No content
            }
            else
            {
                return BadRequest( // 400 Bad request
                $"Customer {id} was found but failed to delete.");
            }
        }
    }
}
