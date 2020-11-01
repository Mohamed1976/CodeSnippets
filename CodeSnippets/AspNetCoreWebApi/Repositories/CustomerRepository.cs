using DataLibrary.BankDemo.Data;
using DataLibrary.BankDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApi.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BankContext bankContext;
        private readonly ILogger<CustomerRepository> logger;
            
        public CustomerRepository(BankContext bankContext, ILogger<CustomerRepository> logger)
        {
            logger.LogInformation("CustomerRepository is called.");
            this.bankContext = bankContext;
            this.logger = logger;
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            logger.LogInformation($"async Task<Customer> CreateAsync(Customer c) is called, " +
                $"FirstName: {customer.FirstName}, LastName: {customer.LastName}");
            
            await bankContext.AddAsync(customer);
            int rowsAffected = await bankContext.SaveChangesAsync();
            return rowsAffected > 0 ? customer : null;
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            logger.LogInformation($"Task<bool?> DeleteAsync(int id), id: {id}.");
            Customer customer = bankContext.Customers.Find(id);
            bankContext.Customers.Remove(customer);
            int rowAffected = await bankContext.SaveChangesAsync();

            return rowAffected > 0;
        }

        public async Task<IEnumerable<Customer>> RetrieveAllAsync()
        {
            logger.LogInformation("Calling, async Task<IEnumerable<Customer>> RetrieveAllAsync().");
            IEnumerable<Customer> customers = await bankContext.Customers.ToListAsync();
            return customers;
        }

        public Task<Customer> RetrieveAsync(int id)
        {
            logger.LogInformation($"public Task<Customer> RetrieveAsync(int id), id = {id}");
            
            Task<Customer> customer = Task.Run<Customer>(() =>
            {
                return bankContext.Customers.Find(id);
            });

            return customer;
        }

        public async Task<Customer> UpdateAsync(int id, Customer customer)
        {
            Customer updatedCustomer = null;

            logger.LogInformation($"public async Task<Customer> UpdateAsync(string id, Customer c), " +
                $"Id: {customer.Id} FirstName: {customer.FirstName}, LastName: {customer.LastName}");

            /* Check if customer exists. */
            Customer customerExists = await RetrieveAsync(id);

            if(customerExists != null)
            {
                logger.LogInformation("Customer exists, updating.");
                bankContext.Update(customer);
                int rowsAffected = await bankContext.SaveChangesAsync();
                updatedCustomer = rowsAffected > 0 ? customer : null;
            }

            return updatedCustomer;
        }
    }
}

/*
// use a static thread-safe dictionary field to cache the customers
private static ConcurrentDictionary<int, Customer> customersCache = null;
// pre-load customers from database as a normal
// Dictionary with CustomerID as the key,
// then convert to a thread-safe ConcurrentDictionary
if (customersCache == null)
{
    logger.LogInformation("CustomerRepository constructor, " +
        "Filling CustomerRepository ConcurrentDictionary cache.");
    customersCache = new ConcurrentDictionary<int, Customer>(
    bankContext.Customers.ToDictionary(c => c.Id));
    IsDirty = false;
}
public bool IsDirty { get; set; }

if(rowsAffected == 1)
{
    IsDirty = true;
    //Because of change tracking we can return the added db customer 
    return c;
}

EntityEntry<Customer> added = await bankContext.AddAsync(c);

if (IsDirty)
{
    customers = await bankContext.Customers.ToListAsync();
    customersCache = new ConcurrentDictionary<int, Customer>
        (customers.ToDictionary(c => c.Id));
}
else
{
    customers = await Task.Run<IEnumerable<Customer>>(() => customersCache.Values);
}

bankContext.Customers

if (IsDirty)
{
logger.LogInformation("IsDirty == true, ConcurrentDictionary is refilled.");
//Update the ConcurrentDictionary  
customersCache = new ConcurrentDictionary<int, Customer>
(bankContext.Customers.ToDictionary(c => c.Id));
IsDirty = false;
}

Customer c;
customersCache.TryGetValue(id, out c);
return c;

EntityEntry<Customer> updated = bankContext.Update(c);
 
if(rowsAffected == 1)
{
IsDirty = true;
return c;
}

return null; 

*/

