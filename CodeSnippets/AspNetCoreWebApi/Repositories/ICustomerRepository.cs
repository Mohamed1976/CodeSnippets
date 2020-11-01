using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary.BankDemo.Models;

namespace AspNetCoreWebApi.Repositories
{
    /*In a real web service, you should use a distributed cache like Redis, an open source data
    structure store, that can be used as a high-performance, high-availability database, cache, or message broker.
    You can read more about Redis at the following link: https://redis.io We will follow good practice and make 
    the repository API asynchronous. It will be instantiated by a Controller class using constructor parameter 
    injection, so a new instance is created to handle every HTTP request:     
     */
    public interface ICustomerRepository
    {
        Task<Customer> CreateAsync(Customer c);
        Task<IEnumerable<Customer>> RetrieveAllAsync();
        Task<Customer> RetrieveAsync(int id);
        Task<Customer> UpdateAsync(int id, Customer c);
        Task<bool?> DeleteAsync(int id);
    }
}
