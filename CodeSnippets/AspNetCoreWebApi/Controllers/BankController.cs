using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DataLibrary.BankDemo.Models;
using System.Net;
using DataLibrary.BankDemo.Data;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebApi.Controllers
{
    [Route("api/bank")]
    [ApiController]
    public class BankController : ControllerBase //Note we can also inherit from ApiController or Controller 
    {
        private readonly ILogger<BankController> logger;
        private readonly BankContext bankContext;

        public BankController(ILogger<BankController> logger, BankContext bankContext)
        {
            this.logger = logger;
            this.bankContext = bankContext;
        }

        /// <summary>
        /// Example call:
        /// https://localhost:5001/api/bank/Customers/10/1/lastName_desc/a
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="sortOrder"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        //Optional URI Parameters and Default Values
        //https://docs.microsoft.com/en-us/aspnet/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2#optional
        //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-3.1#route-constraint-reference
        //https://stackoverflow.com/questions/59258021/the-constraint-reference-string-could-not-be-resolved-to-a-type-netcoreapp3
        //[Route("Customers/{pageSize}/{pageIndex}/{sortOrder}/{searchString:alpha=default}")]
        [Route("Customers/{pageSize}/{pageIndex}/{sortOrder}/{searchString}")]
        [HttpGet]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        //public async Task<IEnumerable<Customer>>
        public async Task<IActionResult> GetCustomers(
            [FromRoute] int pageSize, 
            [FromRoute] int pageIndex,
            [FromRoute] string sortOrder,
            [FromRoute] string searchString)
        {
            logger.LogInformation("Entering GetCustomers, pageSize={0}, pageIndex={1}, sortOrder={2}, " +
                "searchString={3}", pageSize, pageIndex, sortOrder, searchString);

            IQueryable<Customer> customers = bankContext.Customers;

            if (!String.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(s => s.LastName.Contains(searchString)
                || s.FirstName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "firstName_asc":
                    customers = customers.OrderBy(c => c.FirstName);
                    break;
                case "firstName_desc":
                    customers = customers.OrderByDescending(c => c.FirstName);
                    break;
                case "lastName_desc":
                    customers = customers.OrderByDescending(c => c.LastName);
                    break;
                default: // Name ascending 
                    customers = customers.OrderBy(s => s.LastName);
                    break;
            }

            //Check pageIndex  
            pageIndex = pageIndex > 0 ? pageIndex : 1;

            //https://docs.microsoft.com/en-us/ef/core/querying/async
            List <Customer> selectedCustomers = await customers
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToListAsync();

            return Ok(selectedCustomers); ;
        }

        //https://localhost:5001/api/bank/Customers/10/1/firstName_asc
        [Route("Customers/{pageSize}/{pageIndex}/{sortOrder}")]
        [HttpGet]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        //public async Task<IEnumerable<Customer>>
        public async Task<IActionResult> GetCustomers(
            [FromRoute] int pageSize,
            [FromRoute] int pageIndex,
            [FromRoute] string sortOrder)
        {
            logger.LogInformation("Entering GetCustomers, pageSize={0}, pageIndex={1}, sortOrder={2},", 
                pageSize, pageIndex, sortOrder);

            return await GetCustomers(pageSize, pageIndex, sortOrder, "").ConfigureAwait(false);
        }

        //https://localhost:5001/api/bank/Customers/10/1
        [Route("Customers/{pageSize}/{pageIndex}")]
        [HttpGet]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        //public async Task<IEnumerable<Customer>>
        public async Task<IActionResult>
            GetCustomers([FromRoute] int pageSize, [FromRoute] int pageIndex)
        {
            logger.LogInformation("Entering GetCustomers, pageSize={0}, pageIndex={1}",
                pageSize, pageIndex);

            return await GetCustomers(pageSize, pageIndex, default(string), default).ConfigureAwait(false);

            //logger.LogInformation("Entering GetCustomers, pageSize={0}, pageNumber={1}", 
            //    pageSize, pageNumber);
            //await Task.Delay(100);
            //var customers = new List<Customer>()
            //{
            //    new Customer() { FirstName ="Alfa", LastName="__Alfa"},
            //    new Customer() { FirstName ="Beta", LastName="__Beta"},
            //};

            //return Ok(customers); ;            
            //return customers;         
        }

        //https://localhost:5001/api/bank/Customers/5
        [Route("Customers/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        //public async Task<IEnumerable<Customer>>
        public async Task<IActionResult>
            GetCustomerById([FromRoute] int id)
        {
            logger.LogInformation("Entering GetCustomerById, id={0}", id);
            Customer customer = await bankContext.Customers
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            return Ok(customer);
        }

        //https://localhost:5001/api/bank/234/addresses
        [Route("{customerId}/addresses")]
        [HttpGet]
        [ProducesResponseType(typeof(List<Address>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCustomerAddresses([FromRoute] int customerId)
        {
            logger.LogInformation("Entering GetCustomerAddresses, customerId={0}", customerId);
            await Task.Delay(100);
            var addresses = new List<Address>()
            {
                new Address() 
                { 
                    Country="USA", 
                    State="Seatle", 
                    City="Arnhem", 
                    StreetAddress="FirstRoad 101",
                    ZipCode="ALhjnK90"
                },
                new Address()
                {
                    Country="_USA",
                    State="_Seatle",
                    City="_Arnhem",
                    StreetAddress="_FirstRoad 101",
                    ZipCode="_ALhjnK90"
                },
            };

            return Ok(addresses);
        }

        //https://localhost:5001/api/bank/345/emails
        [Route("{customerId}/emails")]
        [HttpGet]
        [ProducesResponseType(typeof(List<Email>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCustomerEmails([FromRoute] int customerId)
        {
            logger.LogInformation("Entering GetCustomerEmails, customerId={0}", customerId);
            await Task.Delay(100);
            var emails = new List<Email>()
            {
                new Email()
                {
                    EmailAddress = "John.Doe@gmail.com"
                },
                new Email()
                {
                    EmailAddress = "Jane.Doe@gmail.com"
                },
            };

            return Ok(emails);
        }

        //https://localhost:5001/api/bank/200/1/transactions
        [Route("{customerId}/{transactionId}/transactions")]
        [HttpGet]
        [ProducesResponseType(typeof(List<Transaction>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAccountTransactions([FromRoute] int customerId, 
            [FromRoute] int transactionId)
        {
            logger.LogInformation("Entering GetAccountTransactions, customerId={0}, transactionId={1}", 
                customerId, transactionId);
            await Task.Delay(100);
            var transactions = new List<Transaction>()
            {
                new Transaction()
                {
                     Amount = 500,
                     Type = TransactionType.Deposit,
                     Description = "Alfa Description",
                },
                new Transaction()
                {
                     Amount = 1800,
                     Type = TransactionType.Deposit,
                     Description = "Beta Description",
                }            
            };

            return Ok(transactions);
        }

        //Note in this example Guid is used as ID  
        //https://localhost:5001/api/bank/customer/9245fe4a-d402-451c-b9ed-9c1a04247482/account/AA45fe4a-d402-451c-b9ed-9c1a04247482
        //Entering GetCustomerAccounts, customerId = 9245fe4a-d402-451c-b9ed-9c1a04247482, accountId=aa45fe4a-d402-451c-b9ed-9c1a04247482
        [Route("customer/{customerId}/account/{accountId}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Account>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCustomerAccounts(
            [FromRoute] Guid customerId,
            [FromRoute] Guid accountId)
        {
            logger.LogInformation("Entering GetCustomerAccounts, customerId={0}, accountId={1}",
                customerId, accountId);
            await Task.Delay(100);

            var accounts = new List<Account>()
            {
                new Account()
                {
                     Balance = 67000,
                     Number ="lsgagahqeytreui73739",
                     Type = AccountType.Checking
                },
                new Account()
                {
                     Balance = 757000,
                     Number ="ABCgahqeytreui73739",
                     Type = AccountType.Savings
                },
            };

            return Ok(accounts);
        }

        //https://localhost:5001/api/bank/9245fe4a-d402-451c-b9ed-9c1a04247482/emails/9245fe4a-d402-451c-b9ed-9c1a04247488
        [Route("{customerId}/emails/{emailId}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> RemoveCustomerEmail(
            [FromRoute] Guid customerId,
            [FromRoute] Guid emailId)
        {
            logger.LogInformation("Entering RemoveCustomerEmail, customerId={0}, emailId={1}",
                customerId, emailId);
            await Task.Delay(100);
            return Ok();
        }

        /* Request body:        
        {
            "emailAddress": "John.Doe@gmail.com",
        } */
        //https://localhost:5001/api/bank/6665fe4a-d402-451c-b9ed-9c1a04247488/changeEmail/5555fe4a-d402-451c-b9ed-9c1a04247488
        [Route("{customerId}/changeEmail/{emailId}")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeCustomerEmail(
            [FromRoute] Guid customerId,
            [FromRoute] Guid emailId,
            [FromBody] Email request)
        {
            logger.LogInformation("Entering ChangeCustomerEmail, customerId={0}, emailId={1}, " +
                "new EmailAddress={2}", customerId, emailId, request.EmailAddress);
            await Task.Delay(100);
            return Ok();
        }

        //https://localhost:5001/api/bank/235/AddEmail
        [Route("{customerId}/AddEmail")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> AddCustomerEmail(
          [FromRoute] int customerId,
          [FromBody] Email request)
        {
            logger.LogInformation("Entering AddCustomerEmail, customerId={0}, " +
                "new EmailAddress={1}", customerId, request.EmailAddress);
            await Task.Delay(100);
            return Created(string.Empty, null);
        }
    }
}
