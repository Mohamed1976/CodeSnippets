using ASPNETCoreWebApp.Data.Entities;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Data
{
    public class CustomersRepository : ICustomersRepository
    {
        public CustomersRepository()
        {
            Initialize();
        }

        private void Initialize()
        {
            var customerFaker = new Faker<Customer>()
                .RuleFor(c => c.Id, (f, c) => f.IndexFaker)
                .RuleFor(c => c.FirstName, (f, c) => f.Name.FirstName())
                .RuleFor(c => c.LastName, (f, c) => f.Name.LastName())
                .RuleFor(c => c.EmailAddress, (f, c) => f.Internet.ExampleEmail(c.FirstName, c.LastName))
                .RuleFor(c => c.Avatar, (f, c) => f.Internet.Avatar());

            Customers = customerFaker.Generate(20);
        }

        public List<Customer> Customers { get; private set; }

        public bool Delete(int Id)
        {
            Customer customer = Customers.Where(c => c.Id == Id).FirstOrDefault();
            bool result = Customers.Remove(customer);
            return result;
        }
    }
}
