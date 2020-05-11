using ASPNETCoreWebApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Data
{
    public interface ICustomersRepository
    {
        List<Customer> Customers { get; }
        bool Delete(int Id);
    }
}
